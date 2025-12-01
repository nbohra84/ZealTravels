using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZealTravel.Infrastructure.Spicejet
{
    class CommonSpiceJet
    {
        /// <summary>
        /// Walks the Navitaire GetBookingResponse XML and extracts:
        ///   • BookingStatus       (BookingInfo/BookingStatus)
        ///   • BalanceDue          (BookingSum/BalanceDue)
        ///   • JourneysCount       (count of Journey elements under Journeys)
        /// </summary>
        public static void RetrieveSpiceJetBookingIdentifiers(
            string responseXml,
            out string bookingStatus,
            out decimal balanceDue,
            out int journeysCount)
        {
            bookingStatus = "";
            balanceDue = 0m;
            journeysCount = 0;

            var xml = new XmlDocument();
            xml.LoadXml(responseXml);
            var root = xml.DocumentElement;
            if (root == null) return;

            // level-1: Envelope → GetBookingResponse
            foreach (XmlNode level1 in root.ChildNodes)
            {
                // level-2: GetBookingResponse → Booking
                var xml1 = new XmlDocument();
                xml1.LoadXml(level1.OuterXml);
                var bookingNode = xml1.DocumentElement;
                if (bookingNode == null || bookingNode.LocalName != "Booking")
                    continue;

                // level-3: children of <Booking>
                foreach (XmlNode child in bookingNode.ChildNodes)
                {
                    var nodeXml = child.OuterXml;

                    // 1) BookingInfo → BookingStatus
                    if (nodeXml.Contains("<BookingInfo"))
                    {
                        var infoDoc = new XmlDocument();
                        infoDoc.LoadXml(nodeXml);
                        var statusNode = infoDoc.SelectSingleNode("//*[local-name()='BookingStatus']");
                        if (statusNode != null)
                            bookingStatus = statusNode.InnerText.Trim();
                    }

                    // 2) BookingSum → BalanceDue
                    if (nodeXml.Contains("<BookingSum"))
                    {
                        var sumDoc = new XmlDocument();
                        sumDoc.LoadXml(nodeXml);
                        var balNode = sumDoc.SelectSingleNode("//*[local-name()='BalanceDue']");
                        if (balNode != null && Decimal.TryParse(balNode.InnerText, out var bal))
                            balanceDue = bal;
                    }

                    // 3) Journeys → count(Journey)
                    if (nodeXml.Contains("<Journeys"))
                    {
                        var jDoc = new XmlDocument();
                        jDoc.LoadXml(nodeXml);
                        var journeyNodes = jDoc.GetElementsByTagName("Journey");
                        journeysCount = journeyNodes?.Count ?? 0;
                    }
                }
            }
        }

        /// <summary>
        /// Walks the Navitaire GetBookingResponse XML and extracts, after payment:
        ///   • BookingStatus       (should be “Confirmed”)
        ///   • BalanceDue          (should be 0)
        ///   • JourneysCount       (count of Journey elements under Journeys, >0)
        ///   • PaymentsCount       (count of Payment elements under Payments, >0)
        /// </summary>
        public static void RetrieveSpiceJetPostPaymentIdentifiers(
            string responseXml,
            out string bookingStatus,
            out decimal balanceDue,
            out int journeysCount,
            out int paymentsCount)
        {
            bookingStatus = "";
            balanceDue = 0m;
            journeysCount = 0;
            paymentsCount = 0;

            var xml = new XmlDocument();
            xml.LoadXml(responseXml);
            var root = xml.DocumentElement;
            if (root == null) return;

            // level-1: GetBookingResponse → its children
            foreach (XmlNode level1 in root.ChildNodes)
            {
                // level-2: Booking node
                var xml1 = new XmlDocument();
                xml1.LoadXml(level1.OuterXml);
                var bookingNode = xml1.DocumentElement;
                if (bookingNode == null || bookingNode.LocalName != "Booking")
                    continue;

                // level-3: children of <Booking>
                foreach (XmlNode child in bookingNode.ChildNodes)
                {
                    var nodeXml = child.OuterXml;

                    // 1) BookingInfo → BookingStatus
                    if (nodeXml.Contains("<BookingInfo"))
                    {
                        var infoDoc = new XmlDocument();
                        infoDoc.LoadXml(nodeXml);
                        var statusNode = infoDoc.SelectSingleNode("//*[local-name()='BookingStatus']");
                        if (statusNode != null)
                            bookingStatus = statusNode.InnerText.Trim();
                    }

                    // 2) BookingSum → BalanceDue
                    if (nodeXml.Contains("<BookingSum"))
                    {
                        var sumDoc = new XmlDocument();
                        sumDoc.LoadXml(nodeXml);
                        var balNode = sumDoc.SelectSingleNode("//*[local-name()='BalanceDue']");
                        if (balNode != null && Decimal.TryParse(balNode.InnerText, out var bal))
                            balanceDue = bal;
                    }

                    // 3) Journeys → count(Journey)
                    if (nodeXml.Contains("<Journeys"))
                    {
                        var jDoc = new XmlDocument();
                        jDoc.LoadXml(nodeXml);
                        var journeyNodes = jDoc.GetElementsByTagName("Journey");
                        journeysCount = journeyNodes?.Count ?? 0;
                    }

                    // 4) Payments → count(Payment)
                    if (nodeXml.Contains("<Payments"))
                    {
                        var pDoc = new XmlDocument();
                        pDoc.LoadXml(nodeXml);
                        var paymentNodes = pDoc.GetElementsByTagName("Payment");
                        paymentsCount = paymentNodes?.Count ?? 0;
                    }
                }
            }
        }
    }
}

'use strict';
const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
//const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");
const CopyPlugin = require("copy-webpack-plugin");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const buildpath = "./ZealTravelWebsite/ZealTravel.Front.Web/wwwroot/assets/";

module.exports = {
    // mode defaults to 'production' if not set
    mode: 'development',

    entry: {
        global: ['./frontend/src/css/global.css'],
        destyle: ['./frontend/src/css/destyle.css'],
        bootstrap: ['./frontend/src/js/bootstrap.min.js', './frontend/src/css/bootstrap.css'],
        ValidationEngine: ['./frontend/src/css/ValidationEngine.css'],
        lightslider: ['./frontend/src/css/lightslider.css'],
        NewEngine: ['./frontend/src/css/NewEngine.css'],
        index: ['./frontend/src/css/index.css'],
        easyAutocomplete: ['./frontend/src/js/jquery.easy-autocomplete.min.js', './frontend/src/css/easy-autocomplete.css'],
        jquryuicss1: ['./frontend/src/css/jquryuicss1.css'],
        select2: ['./frontend/src/js/select2.min.js','./frontend/src/css/select2.css'],
        select2default: ['./frontend/src/css/select2-default.css'],
        homeindex: ['./frontend/src/js/homepage2.js','./frontend/src/css/homeindex.css'],
        loginAgent: ['./frontend/src/js/loginAgent.js', './frontend/src/css/loginAgent.css'],
        about: ['./frontend/src/css/about.css'],
        staticpage: ['./frontend/src/css/staticpage.css'],
        jqueryui1: ['./frontend/src/js/jqueryui1.js'],
        jqueryui2: ['./frontend/src/js/jqueryui2.js'],
        slick: ['./frontend/src/js/slick.js'],
        header: ['./frontend/src/js/header.js'],
        bootstraphoverdropdown: ['./frontend/src/js/bootstrap-hover-dropdown.min.js'],
        onlinejsformvalidationengineen: ['./frontend/src/js/onlinejsformvalidationengine-en.js'],
        search2: ['./frontend/src/js/search2.js'],
        ourServices: ['./frontend/src/js/ourServices.js', './frontend/src/css/ourServices.css'],
        loadingpopup:  ['./frontend/src/css/loadingpopup.css'],
        registerAgency:  ['./frontend/src/js/registerAgency.js','./frontend/src/css/registerAgency.css'],
        registration: ['./frontend/src/js/registration.js'],
        onlineformvalidationengine: ['./frontend/src/js/onlineformvalidationengine.js'],
        changepassword: ['./frontend/src/css/changePassword.css'],
        mydatepicker:  ['./frontend/src/js/mydatepicker.js','./frontend/src/css/mydatepicker.css'],
        AdminLTE: ['./frontend/src/css/AdminLTE.css'],
        table: ['./frontend/src/css/table.css'],
        newdashB: ['./frontend/src/css/newdashB.css'],
        dailyBooking: ['./frontend/src/js/dailybooking.js','./frontend/src/css/dailybooking.css'],
        profile: ['./frontend/src/css/profile.css'],
        colortheme: ['./frontend/src/css/colortheme.css'],
        ledger: ['./frontend/src/js/ledger.js','./frontend/src/css/ledger.css'],
        reportUI: ['./frontend/src/css/reportUI.css'],
        style: ['./frontend/src/css/style.css'],
        newd: ['./frontend/src/css/newd.css'],
        dashboard: ['./frontend/src/js/dashboard.js','./frontend/src/css/dashboard.css'],
        custom: ['./frontend/src/css/custom.css'],
        flightbookings: ['./frontend/src/js/flightbookings.js','./frontend/src/css/flightbookings.css'],
        Ticketreport: ['./frontend/src/js/Ticketreport.js'],
        exporting: ['./frontend/src/js/exporting.js'],
        highcharts: ['./frontend/src/js/highcharts.js'],
        demo: ['./frontend/src/js/demo.js'],
        app: ['./frontend/src/js/app.min.js'],
        oneWay: ['./frontend/src/js/oneway.js','./frontend/src/css/onewaycss.css'],
        mdiengin_k:['./frontend/src/css/mdiengin_k.css'],
        modifysearch_k:['./frontend/src/js/modifysearch_k.js'],
        modifyengine:['./frontend/src/js/modifyengine.js'],
        jqueryui: ['./frontend/src/js/jquery-ui.js','./frontend/src/css/jquery-ui.css'],
        jquerymCustomScrollbar:['./frontend/src/js/jquery.mCustomScrollbar.concat.min.js'],
        bootstrapprogressbar:['./frontend/src/js/bootstrap-progressbar.js'],
        grouping :['./frontend/src/js/grouping.js'],
        Redirect_n_k:['./frontend/src/js/Redirect_n_k.js'],
        templete:['./frontend/src/js/templete.js'] ,
        printTicket: ['./frontend/src/js/print_ticket.js',"./frontend/src/css/printTicket.css"],
        printPassengerPopup: ['./frontend/src/js/PrintPassengerPopup.js'],
        staff: ['./frontend/src/css/staff.css'],
        flightdetailstyle: ['./frontend/src/css/flightdetailstyle.css'],
        thickbox: ['./frontend/src/css/thickbox.css','./frontend/src/js/thickbox.js'],
        flightdisplay: ['./frontend/src/css/flightdisplay.css'],
        flightdinew: ['./frontend/src/css/flightdinew.css'],
        k_flightdisplay: ['./frontend/src/css/k_flightdisplay.css'],
        travellers: ['./frontend/src/css/Travellers.css','./frontend/src/js/travellers.js'],
        k_travellers: ['./frontend/src/js/k_travellers.js'],
        jqueryMaskedinput: ['./frontend/src/js/jquery.maskedinput.js'],
        calanderJs: ['./frontend/src/js/calanderJs.js'],
        payment: ['./frontend/src/css/payment.css','./frontend/src/js/payment.js'],
        printPDF: ['./frontend/src/js/printPDF.js'],
        error:['./frontend/src/css/error.css'],
        countUp:["./frontend/src/js/countUp.js"],
        roundWay:['./frontend/src/css/roundWay.css',"./frontend/src/js/roundWay.js"],
        internationalcss:["./frontend/src/css/internationalcss.css"],
        discountedRoundWay:["./frontend/src/css/discountedRoundWay.css","./frontend/src/js/discountedRoundWay.js"],
        StyleSheetDOM_OneWay:["./frontend/src/css/StyleSheetDOM_OneWay.css"],
        RangeSlider_hotelPlusFlight:["./frontend/src/css/RangeSlider_hotelPlusFlight.css"],
        // pako:["./frontend/src/js/pako.js"],
        multiCity:["./frontend/src/js/c_multicity.js","./frontend/src/css/multiCity.css"],
        mdiengin_n:["./frontend/src/css/mdiengin_n.css"],
        Ticketreport:["./frontend/src/js/Ticketreport.js"],      
    },
    output: {
        filename: 'js/[name].js',
        path: path.resolve(__dirname, buildpath),
    },

    devtool: 'source-map',

    module: {
        rules: [
            {
                test: /\.(css)$/i,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader",
                    // "postcss-loader",
                    // "sass-loader",
                ],
            },
            {
                test: /\.(gif|png|jpe?g|svg)$/i,
                type: 'asset/resource',
                generator: {
                    // adding a hash to the file
                    filename: 'img/[name].[ext]',
                },
            },
            {
                test: /\.(woff(2)?|ttf|eot|svg)$/,
                type: 'asset/resource',
                generator: {
                    filename: 'fonts/[name][ext]',
                },
            },
            {
                test: /\.js$/i,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader",
                },
            },
            {
                test: /\.(hbs|handlebars)$/,
                exclude: /(node_modules)/,
                loader: "handlebars-loader"
            }
        ],
    },
    externals: {
        jquery: 'jQuery',
    },

    plugins: [
        new CleanWebpackPlugin(),
        new CopyPlugin({
            patterns: [
                {
                    from: __dirname + '/frontend/src/img/',
                    to: 'img/',
                    noErrorOnMissing: true
                },
                {
                    from: __dirname + '/frontend/src/js/jquery-1.11.2.min.js',
                    to: 'js/',
                    noErrorOnMissing: true
                },
                {
                    from: __dirname + '/frontend/src/data/statesCities.json',
                    to: 'data/',
                    noErrorOnMissing: true
                },
                {
                    from: __dirname + '/frontend/src/css/prettyPhoto.css',
                    to: 'css/',
                    noErrorOnMissing: true
                 },
                 {
                    from: __dirname + '/frontend/src/css/main.css',
                    to: 'css/',
                    noErrorOnMissing: true
                 },
                 {
                    from: __dirname + '/frontend/src/css/jquery.mCustomScrollbar.css',
                    to: 'css/',
                    noErrorOnMissing: true
                 },
            ]
        }),
        new MiniCssExtractPlugin({
            filename: 'css/[name].css',
        }),
    ],
};

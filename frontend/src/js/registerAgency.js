import _ from "jquery-validation-unobtrusive"
$(document).ready(function () {
    // Clear email error message
    function emailspan() {
        $("#emailerrormsg").html("");
    }

    // Prevent back navigation
    window.history.forward();
    function noBack() {
        window.history.forward();
    }

    // Check checkbox function
    function abcchek() {
        $('#Check').prop('checked', true);
    }


    // Google Translate Initialization
    function googleTranslateElementInit() {
        new google.translate.TranslateElement({ pageLanguage: 'en' }, 'google_translate_element');
    }

    // Toggle visibility of an element and add box shadow
    function myclipopse() {
        var ll = $('#closclippo');
        if (ll.css('display') !== "block") {
            ll.css("display", "block");
            $('#sedax').css("box-shadow", "0 8px 8px rgba(0,0,0,0.5)");
        } else {
            ll.css("display", "none");
            $('#sedax').css("box-shadow", "0 0px 0px rgba(0,0,0,0.5)");
        }
    }

    // Responsive menu toggle
    var touch = $('#resp-menu');
    var menu = $('.menu');

    touch.on('click', function (e) {
        e.preventDefault();
        menu.slideToggle();
    });

    $(window).resize(function () {
        if ($(window).width() > 767 && menu.is(':hidden')) {
            menu.removeAttr('style');
        }
    });

    // $(document).ready(function () {
    //     let fetchData = null;

    //     fetch('/Account/GetAllStates', {
    //         method: 'GET',
    //     })
    //         .then(response => {
    //             if (!response.ok) {
    //                 throw new Error('Network response was not ok');
    //             }
    //             return response.json();
    //         })
    //         .then(data => {
    //             fetchData = data;
    //             const stateDropdown = $('#stateDropdown');
    //             stateDropdown.empty();
    //             stateDropdown.append('<option value="Select State">Select State</option>');
    //             const hiddenState = $('#hiddenState').val(); // The value from the hidden input
    //             var stateCode;
    //             console.log(hiddenState);

    //             //Populate the state dropdown with states
    //             $.each(data, function (index, state) {
    //                 // console.log(state.stateName==hiddenState);
    //                 // if(stateCode.length <= 0) {
    //                 //     stateCode = state.stateName == hiddenState ? state.stateCode : null;
    //                 // }
    //                 // console.log(stateCode);

    //                 stateDropdown.append('<option '+ `${state.stateCode == hiddenState ?' selected ':''}`  + ' value="' + state.stateCode + '">' + state.stateName + '</option>');
    //             });

    //             // Get the selected state from the hidden input
    //             if(stateCode != null){
    //                 $('#stateDropdown').val(stateCode);
    //                 console.log(stateCode); 
    //             }
    //             //$('#stateDropdown').val("Select State");
    //             // if (hiddenState) {
    //             //     // If the hidden state value exists, select it in the dropdown
    //             //     stateDropdown.val(hiddenState);

    //             //     // Trigger the change event to load cities for the selected state
    //             //     $('#stateDropdown').trigger('change');
    //             // }
    //         })
    //         .catch(error => console.log('Error:', error));

    //     // Handle the state dropdown change event
    //     $('#stateDropdown').change(function () {
    //         const selectedState = $(this).val();
    //         const cityDropdown = $('#cityDropdown');
    //         cityDropdown.empty();
    //         cityDropdown.append('<option value="">Select City</option>');

    //         if (selectedState) {
    //             // Fetch cities based on the selected state
    //             $.ajax({
    //                 url: '/Account/GetCityByState',
    //                 type: 'GET',
    //                 data: { stateCode: selectedState },
    //                 success: function (data) {
    //                     const stateData = data.cities.find(state => state.stateCode === selectedState);
    //                     if (stateData && stateData.cities) {
    //                         const cities = stateData.cities;
    //                         if (cities.length) {
    //                             // Populate the city dropdown with cities
    //                             $.each(cities, function (index, city) {
    //                                 cityDropdown.append('<option value="' + city + '">' + city + '</option>');
    //                             });

    //                             // Get the selected city from the hidden input
    //                             const hiddenCity = $('#hiddenCity').val(); // The value from the hidden input

    //                             if (hiddenCity && cities.includes(hiddenCity)) {
    //                                 // If the hidden city value exists and is in the list, select it
    //                                 cityDropdown.val(hiddenCity);
    //                             } else {
    //                                 // Otherwise, select the first city
    //                                 cityDropdown.val(cities[0]);
    //                             }
    //                         } else {
    //                             cityDropdown.append('<option value="">No Cities Available</option>');
    //                         }
    //                     } else {
    //                         cityDropdown.append('<option value="">No Cities Available</option>');
    //                     }
    //                 },
    //                 error: function (jqXHR, textStatus, errorThrown) {
    //                     cityDropdown.append('<option value="">Error loading cities</option>');
    //                 }
    //             });
    //         } else {
    //             cityDropdown.empty().append('<option value="">Select City</option>');
    //         }
    //     });
    // });

    $(document).ready(function () {
        let fetchData = null;
        fetch('/Account/GetAllStates', {
            method: 'GET',
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                fetchData = data;
                const stateDropdown = $('#stateDropdown');
                stateDropdown.empty();
                stateDropdown.append('<option value="Select State">Select State</option>');
                let hiddenState = $('#hiddenState').val(); // The value from the hidden input
                let stateName = null;  // This will store the state name
                // Check if hiddenState is a stateCode, and if so, update it to the corresponding stateName
                $.each(data, function (index, state) {
                    if (state.stateCode === hiddenState) {
                        hiddenState = state.stateName; // Update hiddenState to stateName
                        stateName = state.stateName;   // Store the stateName for selection
                    }
                });
                // Populate the state dropdown with states
                $.each(data, function (index, state) {
                    stateDropdown.append('<option ' + (state.stateName === hiddenState ? 'selected' : '') + ' value="' + state.stateName + '">' + state.stateName + '</option>');
                });
                // Set the value of stateDropdown to the updated hiddenState (stateName)
                $('#stateDropdown').val(hiddenState);
                // Trigger city population based on the selected state
                $('#stateDropdown').trigger('change');
            })
            .catch(error => console.log('Error:', error));
        // Handle the state dropdown change event
        $('#stateDropdown').change(function () {
            const selectedState = $(this).val();
            const cityDropdown = $('#cityDropdown');
            cityDropdown.empty();
            cityDropdown.append('<option value="">Select City</option>');
            if (selectedState) {
                // Fetch cities based on the selected state
                $.ajax({
                    url: '/Account/GetCityByState',
                    type: 'GET',
                    data: { stateName: selectedState }, // Send stateName instead of stateCode
                    success: function (data) {
                        const stateData = data.cities.find(state => state.stateName === selectedState);
                        if (stateData && stateData.cities) {
                            const cities = stateData.cities;
                            if (cities.length) {
                                // Populate the city dropdown with cities
                                $.each(cities, function (index, city) {
                                    cityDropdown.append('<option value="' + city + '">' + city + '</option>');
                                });
                                // Get the selected city from the hidden input
                                const hiddenCity = $('#hiddenCity').val(); // The value from the hidden input
                                if (hiddenCity && cities.includes(hiddenCity)) {
                                    // If the hidden city value exists and is in the list, select it
                                    cityDropdown.val(hiddenCity);
                                } else {
                                    // Otherwise, select the first city
                                    cityDropdown.val(cities[0]);
                                }
                            } else {
                                cityDropdown.append('<option value="">No Cities Available</option>');
                            }
                        } else {
                            cityDropdown.append('<option value="">No Cities Available</option>');
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        cityDropdown.append('<option value="">Error loading cities</option>');
                    }
                });
            } else {
                cityDropdown.empty().append('<option value="">Select City</option>');
            }
        });
    });

});

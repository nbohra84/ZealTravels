import _ from "jquery-validation-unobtrusive"
$(document).ready(function () {
    function forgetpassagency() {
        document.getElementById('myModal2').style.display = 'block';
    }

    function closebut1() {
        document.getElementById('myModal2').style.display = 'none';
    }
    function forgetpasssnhid() {

        document.getElementById("forgetagentpassspan").innerHTML = "";

    }
    function valforgetpassemailagent() {
        //debugger;
        var custemailid = document.getElementById("forgetpassagent").value;
        if (custemailid == null || custemailid == "") {
            document.getElementById("forgetagentpassspan").classList.remove("text-success")
            document.getElementById("forgetagentpassspan").innerHTML =
                "Email is required";
            return false;
        }

        else {

        }
        var email = document.getElementById('forgetpassagent');
        var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

        if (!filter.test(email.value)) {
            document.getElementById("forgetagentpassspan").innerHTML = "Please Enter valid Email";
            email.focus;
            return false;
        }

    }
    if ($("form#forgot-password-form").length) {
        $("form#forgot-password-form .submit").click(function (e) {
            e.preventDefault();
            var isValid = $("form#forgot-password-form").valid();
            if (isValid) {
                // var formData = $(this).serialize();
                const email = document.getElementById("ForgotPasswordEmail").value;

                fetch('/Account/ForgotPassword', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ email })
                })
                    .then(response => {
                        if (response.status == 200) {
                            document.getElementById("forgetagentpassspan").classList.add("text-success")
                            document.getElementById("forgetagentpassspan").innerText = 'Your password reset request has been processed. If the email address is registered in our system, you will receive an email with further instructions. Please check your inbox and spam/junk folder. If you do not receive an email, try again or contact support for assistance.';
                        } else if (response.status == 500) {
                            document.getElementById("forgetagentpassspan").innerText = 'Your password reset request could not be processed at this time. If the issue persists, contact support for further assistance.';
                        } else {
                            document.getElementById("forgetagentpassspan").innerText = 'Your password reset request could not be processed at this time. If the issue persists, contact support for further assistance.';
                        }
                    }
                    )
                    .catch(error => console.log('Error:', error));
            }
        })

    } else {
        return false
    }
    // Handle Enter key press on login form to submit
    if ($("form#loginForm").length) {
        $("form#loginForm input").on("keypress", function(e) {
            if (e.which === 13) { // Enter key
                e.preventDefault();
                $("form#loginForm").submit();
            }
        });
        
        // Ensure form submits properly - handle submit event
        // CRITICAL FIX: Don't block form submission - let it go through
        $("form#loginForm").on("submit", function(e) {
            var form = $(this);
            console.log('🔵 [loginAgent.js] Form submit event triggered');
            
            // Only check validation if jQuery Validation is actually initialized
            if ($.fn.validate && form.data('validator')) {
                console.log('🔵 [loginAgent.js] jQuery Validation is active, checking validity');
                if (!form.valid()) {
                    console.log('🔴 [loginAgent.js] Form validation failed - PREVENTING SUBMIT');
                    e.preventDefault();
                    return false;
                }
                console.log('🟢 [loginAgent.js] Form validation passed');
            } else {
                console.log('🟢 [loginAgent.js] No jQuery Validation active, allowing native HTML5 validation');
                // Use native HTML5 validation
                if (form[0] && form[0].checkValidity && !form[0].checkValidity()) {
                    console.log('🔴 [loginAgent.js] Native validation failed');
                    e.preventDefault();
                    return false;
                }
            }
            
            console.log('🟢 [loginAgent.js] Form will submit normally');
            // Allow form to submit normally - DON'T prevent default
            return true;
        });
    }

    window.forgetpassagency = forgetpassagency;
    window.forgetpasssnhid = forgetpasssnhid;
    window.valforgetpassemailagent = valforgetpassemailagent;
    window.closebut1 = closebut1;
});
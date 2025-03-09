$(document).ready(function () {
    // Handle Login Form Submission
    $("#loginForm").submit(function (e) {
        e.preventDefault(); // Prevent default form submission

        $.ajax({
            url: "/Account/Login", // ✅ FIXED: Correct URL for login
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Email: $("#loginEmail").val(),
                Password: $("#loginPassword").val()
            }),
            success: function (response) {
                if (response.success) {
                    alert("Login successful! Redirecting...");
                    window.location.href = response.redirectUrl; // ✅ Redirect to correct dashboard
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr) {
                alert(xhr.responseText); // Show error message
            }
        });
    });

    // Handle Sign Up Form Submission
    $("#signupForm").submit(function (e) {
        e.preventDefault(); // Prevent default form submission

        $.ajax({
            url: "/Account/Register",  // ✅ FIXED: Correct controller route
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                FullName: $("#signupFullName").val(),
                Email: $("#signupEmail").val(),
                Password: $("#signupPassword").val(),
                Role: $("#signupRole").val() // Ensure this field exists
            }),
            success: function (response) {
                if (response.success) {
                    alert("Registration successful! Redirecting...");
                    window.location.href = "/Home/Index"; // Redirect to homepage
                } else {
                    alert(response.message); // Show error message
                }
            },
            error: function (xhr) {
                alert(xhr.responseText); // Show server error response
            }
        });
    });
});

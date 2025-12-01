document.addEventListener("DOMContentLoaded", function () {
  function toggle(className, displayState) {
    var elements = document.getElementsByClassName(className);

    for (var i = 0; i < elements.length; i++) {
      elements[i].style.display = displayState;
    }
  }

  function handleEditButtonClick(id) {
    toggle("edit-row", "none ");
    toggle("view-row", "flex");
    document.getElementById("edit-row-" + id).style.display = "flex";
    document.getElementById("view-row-" + id).style.display = "none";
    document.getElementById("add-new-row").style.display = "none";
  }
  function handleCancelButtonClick(id) {
    document.getElementById("edit-row-" + id).style.display = "none";
    document.getElementById("view-row-" + id).style.display = "flex";
  }

  document.querySelectorAll(".edit-btn").forEach((button) => {
    button.addEventListener("click", function () {
      var id = this.getAttribute("data-id");
      handleEditButtonClick(id);
    });
  });

  document.querySelectorAll(".cancel-btn").forEach((button) => {
    button.addEventListener("click", function () {
      var id = this.getAttribute("data-id");
      handleCancelButtonClick(id);
    });
  });

  function deleteSupplier(id) {
    var form = document.getElementById("PnrMakeDays_" + id);
    if (form) {
      form.action = "/AdminCredentionalManagement/DeletePnrMakeDays";
      form.submit();
    } else {
      console.error("Form not found for supplier ID:", id);
    }
  }
  window.deleteSupplier = deleteSupplier;

  document.getElementById("btnAddNewRecord").addEventListener("click", function () {
    document.getElementById("add-new-row").style.display = "block";

    // Hide any edit rows when adding a new record
    var editRows = document.querySelectorAll(".edit-row");
    editRows.forEach(function (row) {
      row.style.display = "none";
    });
  });

  // Hide "Add New Row" when clicking the cancel button for the add new row
  document.querySelectorAll(".cancel-add-btn").forEach((button) => {
    button.addEventListener("click", function () {
      document.getElementById("add-new-row").style.display = "none";
    });
  });
});


$(document).ready(function () {
  $(".uppercase-input").on("input", function () {
      $(this).val($(this).val().toUpperCase());
  });
});

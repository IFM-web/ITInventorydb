
    function geturl() {
        let url = window.location.hostname;
    if (url == 'localhost') {
                return '';
            }
    else return '/itinventory';
        }
    let myurl = geturl();
    // Function to open the image in a new tab
    function openImage(imagePath) {
        let baseUrl = geturl();
    let fullUrl = baseUrl + imagePath;
    window.open(fullUrl, '_blank');
        }

    $(document).ready(function() {
        console.log('Document ready');

    // Handle expand/collapse with direct click binding
    $('.expand-button').on('click', function(e) {
        e.preventDefault();
    e.stopPropagation();

    var $button = $(this);
    var materialId = $button.data('material-id');
    var $icon = $button.find('i');
    var $row = $button.closest('tr');

    console.log('Button clicked');
    console.log('Material ID:', materialId);

    if ($icon.hasClass('fa-chevron-circle-right')) {
        // Expanding
        $icon.removeClass('fa-chevron-circle-right').addClass('fa-chevron-circle-down');

    // Show loading indicator
    var loadingRow = `
    <tr class="expandable-row loading-row">
        <td colspan="10" class="text-center py-3">
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
        </td>
    </tr>`;
    $row.after(loadingRow);

    // Make the AJAX call with correct URL
    $.ajax({
        url: myurl + '/Material/GetMaterialItems',
    type: 'GET',
    data: {materialId: materialId },
    success: function(response) {
        console.log('API Response:', response);
        $('.loading-row').remove();
        var response = JSON.parse(response);
        if (response.length>0) {
                                // Get the asset type from the current row
                                const assetType = $row.find('.asset-name').text().trim();
    var detailRow = generateItemsTable(response, assetType);
    $row.after(detailRow);
                            } else {
        toastr.error('No items found or error loading items');
    $icon.removeClass('fa-chevron-circle-down').addClass('fa-chevron-circle-right');
                            }
                        },
    error: function(xhr, status, error) {
        console.error('Ajax Error:', error);
    console.error('Status:', status);
    console.error('Response:', xhr.responseText);
    $('.loading-row').remove();
    $icon.removeClass('fa-chevron-circle-down').addClass('fa-chevron-circle-right');
    toastr.error('Error loading material items');
                        }
                    });
                } else {
        // Collapsing
        $icon.removeClass('fa-chevron-circle-down').addClass('fa-chevron-circle-right');
    $row.next('.expandable-row').remove();
                }
            });

    function generateItemsTable(items, assetType) {
                if (!items || items.length === 0) {
                    return `
    <tr class="expandable-row">
        <td colspan="8" class="text-center py-3">
            No items found for this invoice
        </td>
    </tr>`;
                }

    var content = `
    <tr class="expandable-row">
        <td colspan="12" class="p-0">
            <div class="detail-content p-3">
                <div class="asset-type d-none">${assetType}</div>
                ${generateMaterialItemsTable(items, assetType)}
            </div>
        </td>
    </tr>`;

    return content;
            }

    function generateMaterialItemsTable(items, assetType) {
                //const isComputerAsset = ['Desktop', 'Laptop', 'Server'].includes(assetType);

    // Define columns based on asset type
    const commonColumns = `
    <th>Serial No</th>
    <th>Model No</th>
    <th>Warranty Date</th>
    <th>Status</th>
    `;

    const computerColumns = `
    <th>Generation</th>
    <th>Processor</th>
    <th>RAM (GB)</th>
    <th>HDD</th>
    <th>SSD</th>
    <th>Windows Key</th>
    <th>MS Office Key</th>
    `;

    const otherColumns = `
    <th>Item Name</th>
    <th>Other Details</th>
    `;

        tableContent = "";
        tableContent = `
    <table class="table table-sm table-bordered mb-0">
        <thead class="table-light">
            <tr>
                ${commonColumns}
                
                ${computerColumns}
            </tr>
        </thead>
        <tbody>
            `;
        items.forEach(function (item) {

            if (item.ItemName == "" || item.ItemName==null) {
              
                const commonCells = `
            <td>${item.SerialNo || ''}</td>
            <td>${item.ModelNo || ''}</td>
            <td>${item.WarrantyDate || ''}</td>
            <td>
                <span class="badge ${item.Status === 'Assigned' ? 'bg-success' : 'bg-primary'}">
                    ${item.Status || 'Available'}
                </span>
            </td>
            `;


                const computerCells = `
            <td>${item.Generation || ''}</td>
            <td>${item.Processor || ''}</td>
            <td>${item.RAMCapacity || ''}</td>
            <td>${item.HardDisk || ''}</td>
            <td>${item.SSDCapacity || ''}</td>
            <td>${item.WindowsKey || ''}</td>
            <td>${item.MsOfficeKey || ''}</td>
            `;

                tableContent += `
            <tr>
                ${commonCells}
                ${computerCells}
            </tr>
            `;
            } else {
                 tableContent += `
    <table class="table table-sm table-bordered mb-0">
        <thead class="table-light">
            <tr>
                ${commonColumns}

                ${otherColumns}
            </tr>
        </thead>
        <tbody>
            `;
                const commonCells = `
            <td>${item.SerialNo || ''}</td>
            <td>${item.ModelNo || ''}</td>
            <td>${item.WarrantyDate || ''}</td>
            <td>
                <span class="badge ${item.Status === 'Assigned' ? 'bg-success' : 'bg-primary'}">
                    ${item.Status || 'Available'}
                </span>
            </td>
            `;
            const otherCells = `
            <td>${item.ItemName || ''}</td>
            <td>${item.Other || ''}</td>
            `;
                tableContent += `
            <tr>
                ${commonCells}
                ${otherCells}
            </tr>
            `;
            }
           
                });

            tableContent += `
        </tbody>
    </table>
    `;

    return tableContent;
            }

    // Log initial table state
    console.log('Total rows:', $('#invoiceTable tbody tr.main-row').length);

    // Initialize DataTable
    var table = $('#invoiceTable').DataTable({
        order: [[1, 'desc']],
    pageLength: 25,
    language: {
        search: "Search invoices:"
                },
    columnDefs: [
    {orderable: false, targets: 0 }
    ]
            });

    // Initialize date pickers
    $('.date-picker').datepicker({
        format: 'dd/mm/yyyy',
    autoclose: true,
    todayHighlight: true
            });

    // Show success message if exists
    //@if (TempData["ShowMessage"]?.ToString() == "True")
    //{
    //    <text>
    //        Swal.fire({
    //            title: 'Success!',
    //        text: '@TempData["Success"]',
    //        icon: 'success',
    //        timer: 2000,
    //        showConfirmButton: false
    //            });
    //    </text>
    //}

            // Function to update column visibility based on asset type
    function updateColumnVisibility() {
        $('.table').each(function () {
            const assetType = $(this).closest('.detail-content').find('.asset-type').text();
            const isComputerAsset = ['Desktop', 'Laptop', 'Server'].includes(assetType);

            $(this).find('th.computer-specs, td.computer-specs').toggle(isComputerAsset);
            $(this).find('th.other-specs, td.other-specs').toggle(!isComputerAsset);
        });
            }

    // Initial column visibility update
    updateColumnVisibility();
        });

    function clearFilters() {
        $('#searchTerm').val('');
    $('.date-picker').val('');
    $('#filterForm').submit();
        }


$('#filterInput').on('keyup', function () {
    let filter = $(this).val().toUpperCase();
    let table = $('#invoiceTable tbody'); // Main table's tbody
    let found = false;

    table.children('tr').each(function () {  // Only direct <tr> children
        let row = $(this);
        let match = false;

        // Check each <td> in the row, but ignore <td> containing another table
        row.children('td').each(function () {
            if ($(this).find('table').length === 0) { // Ignore inner tables
                if ($(this).text().toUpperCase().indexOf(filter) > -1) {
                    match = true;
                    return false; // Stop loop if match is found
                }
            }
        });

        if (match) {
            row.show();
            found = true;
        } else {
            row.hide();
        }
    });

    // Remove previous "Not Found" row
    $('#notFoundRow').remove();

    // Add "Not Found" row if no match is found in main table
    if (!found) {
        table.append('<tr id="notFoundRow"><td colspan="100%" class="text-center">Not Found</td></tr>');
    }
});

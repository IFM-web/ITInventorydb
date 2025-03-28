//<script>
    function geturl() {
        let url = window.location.hostname;
    if (url == 'localhost') {
                return '';
            }
    else return '/itinventory';
        }
    let myurl = geturl();

    //$(document).ready(function() {
    //    updateColumnVisibility();
    //)};

/*function ItemsDetails(materialId) {
       // e.preventDefault();
    //e.stopPropagation();

   // var $button = $(this);
    //var materialId = $button.data('material-id');
    //var $icon = $button.find('i');
    //var $row = $button.closest('tr');


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
       
    $('.loading-row').remove();

    if (response.success && response.data) {
                                // Get the asset type from the current row
                                const assetType = $row.find('.asset-name').text().trim();
    var detailRow = generateItemsTable(response.data, assetType);
    $row.after(detailRow);
                            } else {
        toastr.error('No items found or error loading items');
    $icon.removeClass('fa-chevron-circle-down').addClass('fa-chevron-circle-right');
                            }
                        },
    error: function(xhr, status, error) {
        
                        }
                    });
    }


    else {
      
        $icon.removeClass('fa-chevron-circle-down').addClass('fa-chevron-circle-right');
    $row.next('.expandable-row').remove();
                }
            };

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
                const isComputerAsset = ['Desktop', 'Laptop', 'Server'].includes(assetType);

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

    let tableContent = `
    <table class="table table-sm table-bordered mb-0">
        <thead class="table-light">
            <tr>
                ${commonColumns}
                ${isComputerAsset ? computerColumns : otherColumns}
            </tr>
        </thead>
        <tbody>
            `;

            items.forEach(function(item) {
                    const commonCells = `
            <td>${item.serialNo || ''}</td>
            <td>${item.modelNo || ''}</td>
            <td>${item.warrantyDate || ''}</td>
            <td>
                <span class="badge ${item.status === 'Assigned' ? 'bg-success' : 'bg-primary'}">
                    ${item.status || 'Available'}
                </span>
            </td>
            `;

            const computerCells = `
            <td>${item.generation || ''}</td>
            <td>${item.processor || ''}</td>
            <td>${item.ramCapacity || ''}</td>
            <td>${item.hardDisk || ''}</td>
            <td>${item.ssdCapacity || ''}</td>
            <td>${item.windowsKey || ''}</td>
            <td>${item.msOfficeKey || ''}</td>
            `;

            const otherCells = `
            <td>${item.itemName || ''}</td>
            <td>${item.other || ''}</td>
            `;

            tableContent += `
            <tr>
                ${commonCells}
                ${isComputerAsset ? computerCells : otherCells}
            </tr>
            `;
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
   /* @if (TempData["ShowMessage"]?.ToString() == "True")
    {
        <text>
            Swal.fire({
                title: 'Success!',
            text: '@TempData["Success"]',
            icon: 'success',
            timer: 2000,
            showConfirmButton: false
                });
        </text>
    }
    */
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

      

    function clearFilters() {
        $('#searchTerm').val('');
    $('.date-picker').val('');
    $('#filterForm').submit();
        }
//</script>


function ItemsDetails(materialId) {

    $.ajax({
        url: myurl + '/Material/GetMaterialItems',
        type: 'GET',
        data: { materialId: materialId },
        success: (response) => {



        },
        error: (err) => {
            alert(err)
        }
    )};





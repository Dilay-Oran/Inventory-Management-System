using Inventory_Management_System.Data;
using Inventory_Management_System.Models;

namespace Inventory_Management_System.Pages;

public partial class StockPage : ContentPage
{
	public StockPage()
	{
		InitializeComponent();
	}
    private ProductsDB productsdb = new ProductsDB();
    private WarehouseDB warehousedb = new WarehouseDB();
    private StockDB stockdb = new StockDB();

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProducts();
        await LoadWarehouses();
        await LoadStockList();
    }
    private async Task LoadProducts()
    {
        var products = await productsdb.GetAllAsync(); // productdbden ürünleri çekecek
        ProductPicker.ItemsSource = products;// pickerýn içine atacak
        ProductPicker.ItemDisplayBinding = new Binding("ProductsName"); // adýn  picker gözükmesini saðladým
    }
    private async Task LoadWarehouses()
    {
        var warehouses = await warehousedb.GetAllAsync();
        Warehouse.ItemsSource = warehouses;
        Warehouse.ItemDisplayBinding = new Binding("WarehouseDestination");

    }

    private async void SaveButton_OnClicked(object sender, EventArgs e)
    {

        if (ProductPicker.SelectedItem == null ||
      Warehouse.SelectedItem == null ||
      string.IsNullOrWhiteSpace(Quantity.Text))
        {
            await DisplayAlert("Error", "Please fill all fields", "OK");
            return;
        }

        var selectedProduct = (ProductsItem)ProductPicker.SelectedItem;
        var selectedWarehouse = (WarehouseItem)Warehouse.SelectedItem;

        bool isNumber = int.TryParse(Quantity.Text, out int quantity);

        if (!isNumber)
        {
            await DisplayAlert("Error", "Quantity must be a number", "OK");
            return;
        }

        await stockdb.CreateAsync(
            selectedProduct.ProductsName,
            selectedWarehouse.WarehouseDestination,
            quantity
        );

        Quantity.Text = string.Empty;

        await LoadStockList();
        int quantityChange = quantity; // artýk eksi olabilir

        // SADECE ARTIÞTA kapasite kontrolü
        if (quantityChange > 0)
        {
            var currentTotal = await stockdb
                .GetTotalStockInWarehouseAsync(selectedWarehouse.WarehouseDestination);

            if (currentTotal + quantityChange > selectedWarehouse.WarehouseCapacity)
            {
                await DisplayAlert(
                    "Capacity Error",
                    "Warehouse capacity exceeded!",
                    "OK");
                return;
            }
        }

        try
        {
            await stockdb.AddOrUpdateStockAsync(
                selectedProduct.ProductsName,
                selectedWarehouse.WarehouseDestination,
                quantityChange
            );

            Quantity.Text = string.Empty;
            await LoadStockList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }

    }
    private async Task LoadStockList()
    {
        StockListView.ItemsSource = null;
        StockListView.ItemsSource = await stockdb.GetAllAsync();
    }
    private async void DeleteButton_OnClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = button.CommandParameter as StockItem;

        await stockdb.DeleteAsync(item);
        await LoadStockList();
    }


}
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
        // seçilen product
        var selectedProduct = ProductPicker.SelectedItem as ProductsItem;

        // seçilen warehouse
        var selectedWarehouse = Warehouse.SelectedItem as WarehouseItem;

        // quantity kontrolü
        bool isNumber = int.TryParse(Quantity.Text, out int quantity);

        if (selectedProduct == null || selectedWarehouse == null || !isNumber)
        {
            await DisplayAlert("Error", "Please fill all fields correctly.", "OK");
            return;
        }

        await stockdb.CreateAsync(
            selectedProduct.ProductsName,
            selectedWarehouse.WarehouseDestination,
            quantity
        );

        Quantity.Text = string.Empty;

        await LoadStockList();
    }
    private async Task LoadStockList()
    {
        StockListView.ItemsSource = null;
        StockListView.ItemsSource = await stockdb.GetAllAsync();
    }



}
using Inventory_Management_System.Data;

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
    }
    private async Task LoadProducts()
    {
        var products = await productsdb.GetAllAsync(); // productdbden ürünleri çekecek
        ProductPicker.ItemsSource = products;// pickerýn içine atacak
    }
}
namespace PlanetApp
{
    public partial class MainlandCreatePage : ContentPage
    {
        public MainlandCreatePage(TempData tempData)
        {
            InitializeComponent();
            BindingContext = new MainlandCreatePageViewModel(tempData);
        }
    }
}

namespace PlanetApp
{
    public partial class OceanCreatePage : ContentPage
    {
        public OceanCreatePage(TempData tempData)
        {
            InitializeComponent();
            BindingContext = new OceanCreatePageViewModel(tempData);
        }
    }
}

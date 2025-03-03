namespace PlanetApp
{
    public partial class IslandCreatePage : ContentPage
    {
        public IslandCreatePage(TempData tempData)
        {
            InitializeComponent();
            BindingContext = new IslandCreatePageViewModel(tempData);
        }
    }
}

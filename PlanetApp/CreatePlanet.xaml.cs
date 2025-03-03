namespace PlanetApp
{
    public partial class CreatePlanet : ContentPage
    {

        public CreatePlanet()
        {
            InitializeComponent();
        }

        // Наведение для кнопки острова
        private async void OnIslandPointerEntered(object sender, PointerEventArgs e)
        {
            await IslandButton.TranslateTo(0, -50, 250, Easing.CubicInOut);  // Поднимаем кнопку
            await IslandLabel.FadeTo(1, 250, Easing.CubicInOut);  // Плавное появление текста
        }

        private async void OnIslandPointerExited(object sender, PointerEventArgs e)
        {
            await IslandButton.TranslateTo(0, 0, 250, Easing.CubicInOut);  // Возвращаем кнопку на исходное положение
            await IslandLabel.FadeTo(0, 250, Easing.CubicInOut);  // Плавное исчезновение текста
        }

        // Наведение для кнопки материка
        private async void OnMainlandPointerEntered(object sender, PointerEventArgs e)
        {
            await MainlandButton.TranslateTo(0, -50, 250, Easing.CubicInOut);  // Поднимаем кнопку
            await MainlandLabel.FadeTo(1, 250, Easing.CubicInOut);  // Плавное появление текста
        }

        private async void OnMainlandPointerExited(object sender, PointerEventArgs e)
        {
            await MainlandButton.TranslateTo(0, 0, 250, Easing.CubicInOut);  // Возвращаем кнопку на исходное положение
            await MainlandLabel.FadeTo(0, 250, Easing.CubicInOut);  // Плавное исчезновение текста
        }

        // Наведение для кнопки океана
        private async void OnOceanPointerEntered(object sender, PointerEventArgs e)
        {
            await OceanButton.TranslateTo(0, -50, 250, Easing.CubicInOut);  // Поднимаем кнопку
            await OceanLabel.FadeTo(1, 250, Easing.CubicInOut);  // Плавное появление текста
        }

        private async void OnOceanPointerExited(object sender, PointerEventArgs e)
        {
            await OceanButton.TranslateTo(0, 0, 250, Easing.CubicInOut);  // Возвращаем кнопку на исходное положение
            await OceanLabel.FadeTo(0, 250, Easing.CubicInOut);  // Плавное исчезновение текста
        }

    }
}
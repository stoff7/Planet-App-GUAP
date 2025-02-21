using System;
using Microsoft.Maui.Controls;

namespace PlanetApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Анимация при наведении на кнопку "Создание планеты"
        private async void OnPointerEntered(object sender, EventArgs e)
        {
            // Поднимаем кнопку и показываем текст
            await CreateButton.TranslateTo(0, -50, 250, Easing.CubicInOut); // Поднимаем кнопку
            await CreateLabel.FadeTo(1, 250, Easing.SpringIn); // Показываем текст
        }

        private async void OnPointerExited(object sender, EventArgs e)
        {
            // Возвращаем кнопку на место и скрываем текст
            await CreateButton.TranslateTo(0, 0, 250, Easing.CubicInOut); // Опускаем кнопку
            await CreateLabel.FadeTo(0, 250, Easing.SpringOut); // Скрываем текст
        }

        // Анимация при наведении на кнопку "Каталог планет"
        private async void OnCollectionPointerEntered(object sender, EventArgs e)
        {
            // Поднимаем кнопку и показываем текст
            await CollectionButton.TranslateTo(0, -50, 250, Easing.CubicInOut); // Поднимаем кнопку
            await CollectionLabel.FadeTo(1, 250, Easing.SpringIn); // Показываем текст
        }

        private async void OnCollectionPointerExited(object sender, EventArgs e)
        {
            // Возвращаем кнопку на место и скрываем текст
            await CollectionButton.TranslateTo(0, 0, 250, Easing.CubicInOut); // Опускаем кнопку
            await CollectionLabel.FadeTo(0, 250, Easing.SpringOut); // Скрываем текст
        }

    }
}

using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PlanetLib;
using static Microsoft.Maui.Graphics.Colors;
using Microsoft.Maui.Layouts;

namespace PlanetApp
{
    public partial class ViewPlanet : ContentPage
    {
        private List<Planet> _planets;
        private int _currentIndex;
        private int _sortState; // Поле для отслеживания состояния сортировки
        string _filePath = Path.Combine(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        "planets.json"));
        public ViewPlanet()
        {
            InitializeComponent();

            // Инициализация списка планет
            
            _planets = Planet.LoadPlanetsFromJson(_filePath);

            // Установка начального индекса и состояния сортировки
            _currentIndex = 0;
            _sortState = 0;

            ShowPlanet();
        }

        private void ShowPlanet()
        {
            if (_planets == null || _planets.Count == 0)
            {
                PlanetImage.Source = null; // Убираем изображение, если планет нет
                PlanetName.Text = "Ничего не найдено";
                return;
            }

            var planet = _planets[_currentIndex];
            PlanetImage.Source = planet.ImagePath;
            PlanetName.Text = planet.Name;
        }

        private async Task AnimatePlanetChange(string direction)
        {
            var animation = new Animation();

            double oldPlanetExitTranslation = direction == "left" ? Width : -Width;
            double newPlanetEnterTranslation = direction == "left" ? -Width : Width;

            animation.Add(0, 1,
                new Animation(v => PlanetImage.TranslationX = v, 0, oldPlanetExitTranslation));
            animation.Add(0, 1, new Animation(v => PlanetImage.Opacity = v, 1, 0));
            animation.Add(0, 1,
                new Animation(v => PlanetName.TranslationX = v, 0, oldPlanetExitTranslation));
            animation.Add(0, 1, new Animation(v => PlanetName.Opacity = v, 1, 0));

            animation.Commit(this, "PlanetChangeOut", length: 250);
            await Task.Delay(250);

            _currentIndex = direction == "left"
                ? (_currentIndex - 1 + _planets.Count) % _planets.Count
                : (_currentIndex + 1) % _planets.Count;

            ShowPlanet();

            PlanetImage.TranslationX = newPlanetEnterTranslation;
            PlanetImage.Opacity = 0;
            PlanetName.TranslationX = newPlanetEnterTranslation;
            PlanetName.Opacity = 0;

            animation = new Animation();
            animation.Add(0, 1,
                new Animation(v => PlanetImage.TranslationX = v, newPlanetEnterTranslation, 0));
            animation.Add(0, 1, new Animation(v => PlanetImage.Opacity = v, 0, 1));
            animation.Add(0, 1,
                new Animation(v => PlanetName.TranslationX = v, newPlanetEnterTranslation, 0));
            animation.Add(0, 1, new Animation(v => PlanetName.Opacity = v, 0, 1));

            animation.Commit(this, "PlanetChangeIn", length: 500);
        }

        private async void OnLeftArrowClicked(object sender, EventArgs e)
        {
            await AnimatePlanetChange("left");
        }

        private async void OnRightArrowClicked(object sender, EventArgs e)
        {
            await AnimatePlanetChange("right");
        }

        private void PerformSearch(string query)
        {
            var allPlanets = Planet.LoadPlanetsFromJson(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "planets.json"));

            _planets = string.IsNullOrWhiteSpace(query)
                ? allPlanets
                : allPlanets
                    .Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                p.Islands.Any(i => i.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                                p.Mainlands.Any(m => m.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                                p.Oceans.Any(o => o.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) || p.satellites.Any(s => s.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                                )
                    .ToList();

            _currentIndex = _planets.Count > 0 ? 0 : -1; // Устанавливаем индекс -1, если список пуст
            ShowPlanet();
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var query = e.NewTextValue?.ToLower() ?? string.Empty; // Учитываем null и преобразуем в нижний регистр
            PerformSearch(query);
        }

        private void OnSortButtonClicked(object sender, EventArgs e)
        {
            switch (_sortState)
            {
                case 0: // По названию
                    _planets = _planets.OrderBy(p => p.Name).ToList();
                    SortButton.Source = "reversedname.png";
                    _sortState = 1;
                    break;
                case 1: // По названию в обратном порядке
                    _planets = _planets.OrderByDescending(p => p.Name).ToList();
                    SortButton.Source = "number.png";
                    _sortState = 2;
                    break;
                case 2: // По количеству океанов, материков и островов
                    _planets = _planets.OrderByDescending(p => p.Oceans.Count + p.Mainlands.Count + p.Islands.Count + p.satellites.Length).ToList();
                    SortButton.Source = "reversednumber.png";
                    _sortState = 3;
                    break;
                case 3: // По количеству океанов, материков и островов в обратном порядке
                    _planets = _planets.OrderBy(p => p.Oceans.Count + p.Mainlands.Count + p.Islands.Count+p.satellites.Length).ToList();
                    SortButton.Source = "name.png";
                    _sortState = 0;
                    break;
            }

            _currentIndex = 0;
            ShowPlanet();
        }

        private async void GoToEditPlanet(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditPlanet(_planets[_currentIndex]));
        }

        private async void DeletePlanet(object sender, EventArgs e)
        {
            if (_planets.Count == 0) 
            {
                await DisplayAlert("Ошибка", "Планета для уничтожения не найдена", "Ок");
                return;
            }
            string action = await DisplayActionSheet(
                "Вы уверены, что хотите удалить эту планету?", 
                "Отмена", 
                "Удалить");

            if (action == "Удалить")
            {
                // Создаем анимацию взрыва
                await CreateExplosionAnimation();

                // Удаляем планету из списка
                _planets.RemoveAt(_currentIndex);

                // Сохраняем изменения в файл
                Planet.SavePlanetsToJson(_planets, _filePath);

                // Обновляем индекс текущей планеты
                _currentIndex = 0;
                

                await DisplayAlert("Успех", "Планета успешно удалена.", "OK");
                ShowPlanet();
            }
            else
            {
                await DisplayAlert("Отмена", "Удаление отменено.", "OK");
            }
        }

        private async Task CreateExplosionAnimation()
        {
            if (PlanetImage == null) return;

            // Сохраняем изначальные свойства для восстановления
            var originalScale = PlanetImage.Scale;
            var originalOpacity = PlanetImage.Opacity;
            var originalSource = PlanetImage.Source;

    // Анимация сужения
            var shrinkAnimation = new Animation(v => PlanetImage.Scale = v, 1, 0.1);

            // Запускаем анимацию сужения
            shrinkAnimation.Commit(this, "Shrink", length: 500);
            await Task.Delay(500); // Ждем завершения сужения

            // Меняем текстуру на "взрыв" с плавным переходом
            var fadeOutOldTexture = new Animation(v => PlanetImage.Opacity = v, 1, 0);
            fadeOutOldTexture.Commit(this, "FadeOutOld", length: 200);
            await Task.Delay(200); // Ждем завершения исчезновения

            // Устанавливаем новую текстуру
            PlanetImage.Source = "explosion.png";
    await Task.Delay(100);

    // Анимация увеличения и исчезновения
    var expandAnimation = new Animation
    {
        { 0, 0.5, new Animation(v => PlanetImage.Scale = v, 0.3, 10) },
        { 0, 0.5, new Animation(v => PlanetImage.Opacity = v, 1, 0) }
    };

    expandAnimation.Commit(this, "ExpandAndFade", length: 800, finished: (v, c) =>
    {
        
    });

    await Task.Delay(800); // Ждем завершения увеличения и исчезновения
    PlanetImage.Scale = originalScale;
        PlanetImage.Opacity = originalOpacity;
        PlanetImage.Source = originalSource;
        PlanetImage.BackgroundColor = Colors.Transparent;
}



    }
}

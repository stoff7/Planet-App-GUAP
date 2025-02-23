using PlanetLib;
using PlanetApp.ViewModels;

namespace PlanetApp {
public partial class ViewPlanet : ContentPage {
   public ViewPlanet()
   {
      InitializeComponent();

      // Подписываемся на сообщения от ViewModel для запуска анимаций
      MessagingCenter.Subscribe<ViewModels.ViewPlanetViewModel, string>(this,
          "AnimatePlanetChangeOut",
          async (vm, direction) =>
          { await RunExitAnimation(direction); });
      MessagingCenter.Subscribe<ViewModels.ViewPlanetViewModel, string>(this,
          "AnimatePlanetChangeIn",
          async (vm, direction) =>
          { await RunEnterAnimation(direction); });
      MessagingCenter.Subscribe<ViewModels.ViewPlanetViewModel>(this,
          "DeletePlanet",
          async (vm) =>
          { await CreateExplosionAnimation(); });
   }

   private async Task RunExitAnimation(string direction)
   {
      double oldPlanetExitTranslation = direction == "left" ? Width : -Width;
      var animation = new Animation();
      animation.Add(0, 1,
          new Animation(
              v => PlanetImage.TranslationX = v, 0, oldPlanetExitTranslation));
      animation.Add(0, 1, new Animation(v => PlanetImage.Opacity = v, 1, 0));
      animation.Add(0, 1,
          new Animation(
              v => PlanetName.TranslationX = v, 0, oldPlanetExitTranslation));
      animation.Add(0, 1, new Animation(v => PlanetName.Opacity = v, 1, 0));
      animation.Commit(this, "PlanetChangeOut", length: 250);
      await Task.Delay(250);
   }

   private async Task RunEnterAnimation(string direction)
   {
      double newPlanetEnterTranslation = direction == "left" ? -Width : Width;
      // Устанавливаем начальные значения для входящей анимации
      PlanetImage.TranslationX = newPlanetEnterTranslation;
      PlanetImage.Opacity = 0;
      PlanetName.TranslationX = newPlanetEnterTranslation;
      PlanetName.Opacity = 0;

      var animation = new Animation();
      animation.Add(0, 1,
          new Animation(
              v => PlanetImage.TranslationX = v, newPlanetEnterTranslation, 0));
      animation.Add(0, 1, new Animation(v => PlanetImage.Opacity = v, 0, 1));
      animation.Add(0, 1,
          new Animation(
              v => PlanetName.TranslationX = v, newPlanetEnterTranslation, 0));
      animation.Add(0, 1, new Animation(v => PlanetName.Opacity = v, 0, 1));
      animation.Commit(this, "PlanetChangeIn", length: 500);
      await Task.Delay(500);
   }

   private async Task CreateExplosionAnimation()
   {
      if (PlanetImage == null)
         return;

      // Сохраняем изначальные визуальные свойства для восстановления
      var originalScale = PlanetImage.Scale;
      var originalOpacity = PlanetImage.Opacity;

      // Анимация сужения (уменьшения размера)
      var shrinkAnimation = new Animation(v => PlanetImage.Scale = v, 1, 0.1);
      shrinkAnimation.Commit(this, "Shrink", length: 500);
      await Task.Delay(500); // Ждём завершения анимации сужения

      // Анимация исчезновения старой текстуры
      var fadeOutOldTexture = new Animation(v => PlanetImage.Opacity = v, 1, 0);
      fadeOutOldTexture.Commit(this, "FadeOutOld", length: 200);
      await Task.Delay(200); // Ждём завершения исчезновения

      // Обновляем источник изображения через модель – устанавливаем текстуру
      // "взрыв"
      if (BindingContext is ViewPlanetViewModel vm) {
         vm.CurrentPlanet.ImagePath = "explosion.png";
      }
      await Task.Delay(100);

      // Анимация увеличения (масштабирование) и одновременного исчезновения
      var expandAnimation = new Animation {
         { 0, 0.5, new Animation(v => PlanetImage.Scale = v, 0.3, 10) },
         { 0, 0.5, new Animation(v => PlanetImage.Opacity = v, 1, 0) }
      };
      expandAnimation.Commit(this, "ExpandAndFade", length: 800);
      await Task.Delay(
          800); // Ждём завершения анимации увеличения и исчезновения

      // Восстанавливаем исходные свойства анимации
      if (BindingContext is ViewPlanetViewModel vm2) {
         vm2.CurrentPlanet.ImagePath = null;
      }
      PlanetImage.Scale = originalScale;
      PlanetImage.Opacity = originalOpacity;
      PlanetImage.BackgroundColor = Colors.Transparent;
   }
}
}

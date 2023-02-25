using System.Collections.Generic;

namespace CryptoApp.Module.ViewModel
{
    public partial class MainWindowViewModel : Base.ViewModel
    {
        //Lang Dictinary
        private static readonly Dictionary<string, string[]> LANG_VALUE = new Dictionary<string, string[]>()
        {
            {"En",new string[] {"Result","More Info","Name","Symbol", "Current Price", "Price Change 24h", "Full Info", "Assets","To","Menu", "Base Converter", "Settings", "Max Item", "Finder", //13
                "Description", "Assets ID", "Current Price", "Website", "Status", "Pegged", "Volume 24 h","Change 1 hours","Change 24 hours","Change 7 day","Created At","Updated At","Circulating supply","Max supply","Market cap",
                "Total supply","Full diluted market cap","Go To Site","Count Point Min=2 Max=2000","Cryptographic","Lang","Themes","Trade" } },
             {"Укр",new string[] {"Результат","Докладніше","Назва","Символ", "Поточна ціна", "Зміна ціни 24h", "Вся інформація", "Монети","К","Меню", "Базова конвертація", " Налаштування", "Максимум елементів", "Пошук",
                 "Опис", "Монети ID", "Текуця ціна", "Веб сайт", "Статус", "Прив'язаний", "Обсяг за 24 години", "Зміни за 1 годину", "Зміни за 24 години", "Зміни за 7 днів","Створено в","Оновлено в","Зворотна пропозиція","Максимальна пропозиція","Ринкова капіталізація",
                 "Загальна пропозиція","Повна розводнена ринкова капіталізація","На сайт","Кількість точок Мінімум=2 Максимум=2000","Крипто-графік","Мова","Тема","Трейд" } },
              {"Ру",new string[] {"Результат","Подробнее","Название","Символ", "Текущяя цена", "Изменение цены 24h", "Вся информация", "Монеты","К","Меню", "Базовая конвертация", "Настройки", "Максимум элементов", "Поиск",
                "Описание", "Монеты ID", "Текуцяя цена", "Веб сайт", "Статус", "Привязан", "Обём за 24 часа","Изменения за 1 час","Изменения за 24 час","Изменения за 7 дней","Создано в","Обновленно в","Оборотное предложение","Максимальное предложение","Рыночная капитализация",
                "Общее предложение","Полная разводненная рыночная капитализация","На сайт","Количество точек Минимум=2 Максимум=2000","Крипто-график","Язык","Тема","Трейд" } },
        };
        private static readonly Dictionary<string, string[]> ERROR_VALUE = new Dictionary<string, string[]>()
        {
            {"En",new string[] {"No internet","Error"} },
             {"Укр",new string[] {"Немае інтернет звязку", "Помилка" } },
              {"Ру",new string[] {"Нет интернета", "Ошибка" } },
        };
    }
}

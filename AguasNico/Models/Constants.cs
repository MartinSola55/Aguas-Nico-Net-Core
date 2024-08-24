using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models
{
    #region Classes

    public class Constants
    {
        public const string Admin = "ADMIN";
        public const string Dealer = "DEALER";
    }
    public class WppMessages
    {
        public const string ConfirmOrder = "se ha confimardo su pedido. Gracias por elegirnos.";
    }
    public enum State
    {
        [Display(Name = "Pendiente")] Pending = 0,
        [Display(Name = "Confirmado")] Confirmed = 1,
        [Display(Name = "No estaba")] Ausent = 2,
        [Display(Name = "No necesitaba")] NotNeeded = 3,
        [Display(Name = "De vacaciones")] Holidays = 4
    }
    public enum ProductType
    {
        [Display(Name = "Bidón 20L")] B20L = 1,
        [Display(Name = "Bidón 12L")] B12L = 2,
        [Display(Name = "Soda")] Soda = 3,
        [Display(Name = "Máquina frío calor")] Máquina = 4,
    }
    public enum Day
    {
        Lunes = 1,
        Martes = 2,
        Miércoles = 3,
        Jueves = 4,
        Viernes = 5
    }
    public enum InvoiceType
    {
        A = 1,
        B = 2
    }
    public enum TaxCondition
    {
        [Display(Name = "Responsable Inscripto")] RI = 1,
        [Display(Name = "Monotributo")] MO = 2,
        [Display(Name = "Exento")] EX = 3,
        [Display(Name = "Consumidor Final")] CF = 4,
    }

    #endregion

    #region Methods
    public class ConstantsMethods
    {
        // States
        public List<State> GetStates()
        {
            return Enum.GetValues(typeof(State)).Cast<State>().ToList();
        }
        public List<SelectListItem> GetStatesDropdown(SelectListItem firstItem, bool valueString)
        {
            var list = Enum.GetValues(typeof(State)).Cast<State>().Select(x => new SelectListItem
            {
                Text = x.GetDisplayName(),
                Value = valueString ? x.GetDisplayName() : ((int)x).ToString()
            }).ToList();

            list.Insert(0, firstItem);

            return list;
        }

        // Product Types
        public List<ProductType> GetProductTypes()
        {
            return Enum.GetValues(typeof(ProductType)).Cast<ProductType>().ToList();
        }
        public List<SelectListItem> GetProductTypesDropdown(SelectListItem firstItem, bool valueString)
        {
            var list = Enum.GetValues(typeof(ProductType)).Cast<ProductType>().Select(x => new SelectListItem
            {
                Text = x.GetDisplayName(),
                Value = valueString ? x.GetDisplayName() : ((int)x).ToString()
            }).ToList();

            list.Insert(0, firstItem);

            return list;
        }

        // Days
        public List<Day> GetDays()
        {
            return Enum.GetValues(typeof(Day)).Cast<Day>().ToList();
        }
        public List<SelectListItem> GetDaysDropdown(SelectListItem firstItem, bool valueString, bool selectByDay = false)
        {
            var list = Enum.GetValues(typeof(Day)).Cast<Day>().Select(x => new SelectListItem
            {
                Text = x.ToString(),
                Value = valueString ? x.ToString() : ((int)x).ToString(),
                Selected = selectByDay && DateTime.UtcNow.AddHours(-3).DayOfWeek == (DayOfWeek)x
            }).ToList();

            list.Insert(0, firstItem);

            return list;
        }


        // Invoice Types
        public List<InvoiceType> GetInvoiceTypes()
        {
            return Enum.GetValues(typeof(InvoiceType)).Cast<InvoiceType>().ToList();
        }
        public List<SelectListItem> GetInvoiceTypesDropdown(SelectListItem firstItem, bool valueString)
        {
            var list = Enum.GetValues(typeof(InvoiceType)).Cast<InvoiceType>().Select(x => new SelectListItem
            {
                Text = x.ToString(),
                Value = valueString ? x.ToString() : ((int)x).ToString()
            }).ToList();

            list.Insert(0, firstItem);

            return list;
        }


        // Tax Conditions
        public List<TaxCondition> GetTaxConditions()
        {
            return Enum.GetValues(typeof(TaxCondition)).Cast<TaxCondition>().ToList();
        }
        public List<SelectListItem> GetTaxConditionsDropdown(SelectListItem firstItem, bool valueString)
        {
            var list = Enum.GetValues(typeof(TaxCondition)).Cast<TaxCondition>().Select(x => new SelectListItem
            {
                Text = x.GetDisplayName(),
                Value = valueString ? x.GetDisplayName() : ((int)x).ToString()
            }).ToList();

            list.Insert(0, firstItem);

            return list;
        }
    }

    // Mostrar el nombre de la enumeración en lugar del nombre del campo
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var displayAttribute = value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                as DisplayAttribute[];

            return displayAttribute[0].Name;
        }
    }

    #endregion
}

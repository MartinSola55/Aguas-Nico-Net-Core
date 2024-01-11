using System.ComponentModel.DataAnnotations;

namespace AguasNico.Models
{
    public class Constants
    {
        public const string Admin = "ADMIN";
        public const string Dealer = "DEALER";
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
}

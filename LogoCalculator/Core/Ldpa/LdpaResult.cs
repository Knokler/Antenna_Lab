
namespace LogoCalculator.Core.Ldpa
{
    /// <summary>
    /// результаты расчета LDPA антенны, включающий список расчитанных вибраторов и их параметров.
    /// </summary>
    public class LdpaResult
    {
        /// <summary>
        /// Общая длина антенны в мм.
        /// </summary>
        public double TotalLengthMm { get; set; }

        /// <summary>
        /// Список расчитанных вибраторов LDPA
        /// </summary>
        public List<LdpaElement> Elements { get; set; } = new();
        /// <summary>
        /// Максимальная ширина антенны по саммому длиному вибратору.
        /// </summary>
        public double TotalWidthMm { get; set; }

        /// <summary>
        /// Саммый длинный вибратор
        /// </summary>
        public double LongestElementLengthMm { get; set; }

        /// <summary>
        /// Саммый короткий вибратор
        /// </summary>
        public double ShortestElementLengthMm { get; set; }

        /// <summary>
        /// Половинный угол раскрытия антенны в градусах
        /// </summary>
        public double AlphaDeg {  get; set; }

        /// <summary>
        /// Полный угол раскрытия антенны в градусах
        /// </summary>
        public double ApexAngleDeg { get; set; }
        /// <summary>
        /// Оценочная ширина диаграммы направленности в градусах
        /// Грубая инженерная оценка
        /// </summary>
        public double EstimatedGainDbi { get; set; }

        /// <summary>
        /// Предупреждения расчета.
        /// </summary>
        public List<string> Warnings { get; set; } = new();


    }
}

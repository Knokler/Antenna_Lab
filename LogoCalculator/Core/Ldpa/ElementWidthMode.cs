
namespace LogoCalculator.Core.Ldpa
{
    /// <summary>
    /// Режим расчета плоских вибраторов антенны LDPA, определяющий, как будет задаваться ширина элементов.
    /// </summary>
    public enum ElementWidthMode
    {
        /// <summary>
        /// Все вибраторы имеют одинаковую ширину, заданную параметром FirstElementWidthMm. Этот режим подходит для простых конструкций, где ширина элементов не изменяется в зависимости от их длины.
        /// </summary>
        Constant,

        /// <summary>
        /// Ширина каждого следущего вибратора уменьшаеться по коэфиценту ТАУ  
        /// </summary>
        ScaledByTau,

        /// <summary>
        /// Ширина вибратора считается как процент от его длины.
        /// </summary>
        ProportionalToLength,

        /// <summary>
        /// Ширину кажого элемента задает пользыватель.
        /// </summary>
        Custom
    }
}

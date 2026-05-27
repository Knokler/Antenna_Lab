
namespace LogoCalculator.Core.Ldpa
{
    /// <summary>
    /// один вибратор LDPA
    /// </summary>
    public class LdpaElement
    {
        
        
        
        /// <summary>
        /// Порядковый номер элемента
        /// 1 самый длинный элемент, 2 - второй по длинне и т.д.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Частота настройки элемента в Мгц.
        /// </summary>
        public double FrequencyMHz { get; set; }
        
        /// <summary>
        /// Полная длинна вибратора в мм.
        /// </summary>
        public double FullLengthMm { get; set; }
        /// <summary>
        /// Координата центра элемента вдоль оси антенны в мм.
        /// </summary>
        public double XCenterMm { get; set; }

        /// <summary>
        /// Признак реверса фазы
        /// В будущем пригодиться для создания чертежа антенны
        /// </summary>
        public bool IsPhaseReversed { get; set; }
        
        /// <summary>
        /// Длина одного плеча вибратора в мм.
        /// Для симметричного диполя равна половине полной длины.
        /// </summary>
        public double ArmLengthMm { get; set; }

        /// <summary>
        /// Ширина плоского вибратора в мм.
        /// В будущем нужна для лазерной резки.
        /// </summary>
        public double WidthMm { get; set; }

        /// <summary>
        /// Расстояние от этого элемента до следующего в мм.
        /// </summary>
        public double SpacingToNextMm { get; set; }

    }
}

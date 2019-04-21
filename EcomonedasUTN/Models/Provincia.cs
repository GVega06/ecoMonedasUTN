using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EcomonedasUTN.Models
{
    public enum Provincia
    {
 
        [DescriptionAttribute("San José")]
        SanJosé = 1,
        [DescriptionAttribute("Alajuela")]
        Alajuela = 2,
        [DescriptionAttribute(" Cartago")]
        Cartago = 3,
        [DescriptionAttribute("Heredia")]
        Heredia = 4,
        [DescriptionAttribute("Guanacaste")]
        Guanacaste = 5,
        [DescriptionAttribute("Puntarenas")]
        Puntarenas = 6,
        [DescriptionAttribute("Limón")]
        Limón = 7,


    }
}
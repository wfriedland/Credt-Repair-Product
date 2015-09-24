using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3
{
    public class ParceThoughDB
    {
        public List<Debt> Parce(Array[] db)
        {
            Debt debt = new Debt();
            ConvertToClass convert = new ConvertToClass();
            List<Debt> debtList = new List<Debt>();

            for (int i = 0; i < db.Length; i++)
            {
                //debt = convert.ConvertToClass(userDebt);
                debtList[i] = debt;
            }
            return debtList;
         }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optivem.Kata.Banking.Core.UseCases.DepositFunds
{
    public class DepositFundsRequest
    {
        public string AccountNumber { get; set; }
        public int Amount { get; set; }
    }
}

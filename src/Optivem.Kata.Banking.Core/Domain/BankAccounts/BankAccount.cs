﻿using Optivem.Kata.Banking.Core.Exceptions;

namespace Optivem.Kata.Banking.Core.Domain.BankAccounts
{
    public class BankAccount
    {
        public BankAccount(AccountId accountId, AccountNumber accountNumber, AccountHolderName accountHolderName, DateOnly openingDate, Balance balance)
        {
            GuardAgainstEmpty(accountId, ValidationMessages.AccountIdEmpty);
            GuardAgainstEmpty(accountNumber, ValidationMessages.AccountNumberEmpty);
            GuardAgainstEmpty(accountHolderName, ValidationMessages.AccountHolderNameEmpty);

            AccountId = accountId;
            AccountNumber = accountNumber;
            AccountHolderName = accountHolderName;
            OpeningDate = openingDate;
            Balance = balance;
        }

        private static void GuardAgainstEmpty<T>(T value, string message) where T : struct
        {
            if(value.Equals(default(T)))
            {
                throw new ValidationException(message);
            }
        }

        public BankAccount(BankAccount other)
            : this(other.AccountId, other.AccountNumber, other.AccountHolderName, other.OpeningDate, other.Balance)
        {
        }

        public AccountId AccountId { get; }
        public AccountNumber AccountNumber { get; }
        public AccountHolderName AccountHolderName { get; }
        public DateOnly OpeningDate { get; }
        public Balance Balance { get; private set; }

        public void Withdraw(TransactionAmount amount)
        {
            if (Balance.IsLessThan(amount))
            {
                throw new ValidationException(ValidationMessages.InsufficientFunds);
            }

            Balance = Balance.Subtract(amount);
        }

        public void Deposit(TransactionAmount amount)
        {
            Balance = Balance.Add(amount);
        }
    }
}
﻿using FluentAssertions;
using Optivem.Kata.Banking.Core.Domain.BankAccounts;
using Optivem.Kata.Banking.Core.Exceptions;
using Optivem.Kata.Banking.Core.UseCases.OpenAccount;
using Optivem.Kata.Banking.Infrastructure.Fake.Generators;
using Optivem.Kata.Banking.Test.Common.Builders.RequestBuilders;
using Optivem.Kata.Banking.Test.Common.DataEnumerables;
using Optivem.Kata.Banking.Test.Common.Givens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using static Optivem.Kata.Banking.Test.Common.Builders.RequestBuilders.OpenAccountRequestBuilder;

namespace Optivem.Kata.Banking.Test.UseCases
{
    public class OpenAccountUseCaseTest
    {
        private readonly FakeAccountNumberGenerator _accountNumberGenerator;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly OpenAccountUseCase _useCase;

        public OpenAccountUseCaseTest()
        {
            _accountNumberGenerator = new FakeAccountNumberGenerator();
            _useCase = new OpenAccountUseCase(_accountNumberGenerator);
        }

        [Theory]
        [InlineData("John", "Smith", 0, "GB41OMQP68570038161775")]
        [InlineData("Mary", "McDonald", 50, "GB36BMFK75394735916876")]
        public async Task Should_open_account_given_valid_request(string firstName, string lastName, int balance, string accountNumber)
        {
            _accountNumberGenerator.SetupNext(accountNumber);

            var request = AnOpenAccount()
                .FirstName(firstName)
                .LastName(lastName)
                .Balance(balance)
                .Build();

            var expectedResponse = new OpenAccountResponse
            {
                AccountNumber = accountNumber,
            };

            var response = await _useCase.HandleAsync(request);

            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Theory]
        [ClassData(typeof(EmptyStringDataEnumerable))]
        public async Task Should_throw_exception_given_empty_first_name(string firstName)
        {
            var request = AnOpenAccount().FirstName(firstName).Build();

            Func<Task> action = () => _useCase.HandleAsync(request);

            await action.Should().ThrowAsync<ValidationException>()
                .WithMessage(ValidationMessages.FirstNameEmpty);
        }

        [Theory]
        [ClassData(typeof(EmptyStringDataEnumerable))]
        public async Task Should_throw_exception_given_empty_last_name(string lastName)
        {
            var request = AnOpenAccount().LastName(lastName).Build();

            Func<Task> action = () => _useCase.HandleAsync(request);

            await action.Should().ThrowAsync<ValidationException>()
                .WithMessage(ValidationMessages.LastNameEmpty);
        }

        [Theory]
        [ClassData(typeof(NegativeIntDataEnumerable))]
        public async Task Should_throw_exception_given_negative_initial_balance(int balance)
        {
            var request = AnOpenAccount().Balance(balance).Build();

            Func<Task> action = () => _useCase.HandleAsync(request);

            await action.Should().ThrowAsync<ValidationException>()
                .WithMessage(ValidationMessages.BalanceNegative);
        }
    }
}

﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniApp1.Api.Requirement
{
    public class BirthDayRequirement : IAuthorizationRequirement
    {
        public BirthDayRequirement(int age)
        {
            Age = age;
        }

        public int Age { get; set; }


    }
    public class BirthDayRequirementHandler : AuthorizationHandler<BirthDayRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BirthDayRequirement requirement)
        {
            var birthdate = context.User.FindFirst("birth-date");
            if (birthdate == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var today = DateTime.Now;
            var age = today.Year - Convert.ToDateTime(birthdate.Value).Year;

            if (age>=requirement.Age)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();

            }
            return Task.CompletedTask;

        }
    }
}

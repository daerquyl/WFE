﻿using Microsoft.AspNetCore.Mvc;
using Timesheet.Application.Employees.Queries;
using Timesheet.Application.Employees.Services;
using Timesheet.Application.Workflow;
using Timesheet.Domain.Models.Employees;
using Timesheet.Web.Api.ViewModels;

namespace Timesheet.Web.Api.Controllers
{
    public abstract class WorkflowBaseController<TController>: BaseController<TController>
    {
        private readonly IQueryEmployee _employeeQuery;
        private readonly IWorkflowService _workflowService;
        private readonly IEmployeeHabilitation _habilitations;

        public WorkflowBaseController(
            IQueryEmployee employeeQuery,
            IWorkflowService workflowService,
            IEmployeeHabilitation habilitations,
            ILogger<TController> logger): base(logger)
        {
            _employeeQuery = employeeQuery;
            _workflowService = workflowService;
            _habilitations = habilitations;
        }

        protected async Task<WithHabilitations<T>> SetAuthorizedTransitions<T>(T data, Type entityType, Enum status, User currentUser, string employeeId)
        {
            var dataWithHabilitations = new WithHabilitations<T>(data);
            if(data is null)
            {
                return dataWithHabilitations;
            }

            var authorRole = await _habilitations.GetEmployeeRoleOnData(currentUser.Id, employeeId, currentUser.IsAdministrator);

            var authorizedTransitions = _workflowService.NextTranstitions(entityType, status, authorRole);

            if(authorizedTransitions is not null && authorizedTransitions.Any())
            {
                dataWithHabilitations.AuthorizedActions = authorizedTransitions
                    .Select(a => new AuthorizedAction(Convert.ToInt32(a), a.ToString())).ToList();
            }

            return dataWithHabilitations;
        }


        protected async Task<WithHabilitations<U>> CombineAuthorizedTransitions<U, T>(WithHabilitations<U> currentDataWithHabilitations, 
            T data, Type entityType, Enum status, User currentUser, string employeeId)
        {
            var dataWithHabilitation = await SetAuthorizedTransitions(data, entityType, status, currentUser, employeeId);
                
            currentDataWithHabilitations.AuthorizedActions = currentDataWithHabilitations.AuthorizedActions
                .Union(dataWithHabilitation.AuthorizedActions)
                .ToList();

            return currentDataWithHabilitations;
        }

        protected async Task<WithHabilitations<T>> CombineIntersectAuthorizedTransitions<T>(WithHabilitations<T> currentDataWithHabilitations,
    dynamic data, Type entityType, Enum status, User currentUser, string employeeId)
        {
            var authorizedActions = new List<AuthorizedAction>();
            if(data is not null)
            {
                var dataWithHabilitation = await SetAuthorizedTransitions(data, entityType, status, currentUser, employeeId);
                authorizedActions = dataWithHabilitation.AuthorizedActions;
            }

            currentDataWithHabilitations.AuthorizedActions = currentDataWithHabilitations.AuthorizedActions
                .Intersect(authorizedActions)
                .ToList();

            return currentDataWithHabilitations;
        }
    }
}

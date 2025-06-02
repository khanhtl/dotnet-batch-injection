using DotnetBatchInjection.Services;
using Microsoft.AspNetCore.Mvc;
namespace DotnetBatchInjection.Apis;

public static class BaseApi
{
    public static RouteGroupBuilder MapCrudApi<T>(this RouteGroupBuilder group, string routePrefix, Func<ApiServices, IBaseService<T>> serviceSelector)
        where T : Entity
    {
        group.MapGet($"{routePrefix}", async ([AsParameters] ApiServices services) =>
        {
            var service = serviceSelector(services);
            var items = await service.GetAllAsync();
            return Results.Ok(items);
        });

        group.MapGet($"{routePrefix}/{{id:guid}}", async ([AsParameters] ApiServices services, [FromRoute] Guid id) =>
        {
            var service = serviceSelector(services);
            var item = await service.GetByIdAsync(id);
            return item != null ? Results.Ok(item) : Results.NotFound();
        });

        group.MapPost($"{routePrefix}", async ([AsParameters] ApiServices services, [FromBody] T item) =>
        {
            if (item == null)
            {
                return Results.BadRequest();
            }

            var service = serviceSelector(services);
            var result = await service.CreateAsync(item);
            return Results.Ok(result);
        });

        group.MapPut($"{routePrefix}/{{id:guid}}", async ([AsParameters] ApiServices services, [FromRoute] Guid id, [FromBody] T item) =>
        {
            item.Id = id;
            var service = serviceSelector(services);
            var result = await service.UpdateAsync(item);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });

        group.MapDelete($"{routePrefix}/{{id:guid}}", async ([AsParameters] ApiServices services, [FromRoute] Guid id) =>
        {
            var service = serviceSelector(services);
            var success = await service.DeleteAsync(id);
            return success ? Results.Ok() : Results.NotFound();
        });

        return group;
    }

    public static RouteGroupBuilder MapApplicationApi(this RouteGroupBuilder group, string routePrefix, Func<ApiServices, IBaseService<Application>> serviceSelector)
    {
        group.MapPost($"{routePrefix}", async ([AsParameters] ApiServices services, [FromBody] Application application) =>
        {
            if (application == null || application.CandidateId == Guid.Empty || application.JobId == Guid.Empty)
            {
                return Results.BadRequest();
            }

            var candidate = await services.CandidateService.GetByIdAsync(application.CandidateId);
            var job = await services.JobService.GetByIdAsync(application.JobId);
            if (candidate == null || job == null)
            {
                return Results.BadRequest();
            }

            var service = serviceSelector(services);
            var result = await service.CreateAsync(application);
            return Results.Ok(result);
        });

        return group.MapCrudApi(routePrefix, serviceSelector);
    }
}

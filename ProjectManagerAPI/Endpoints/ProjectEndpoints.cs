namespace ProjectManagerAPI.Endpoints;

using ProjectManagerAPI.Services;
using ProjectManagerAPI.DTOs;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this WebApplication app)
    {
        app.MapGet("/projects", GetAllProjects)
            .RequireAuthorization();

        app.MapGet("/projects/{id:guid}", GetProjectById)
            .RequireAuthorization();

        app.MapPost("/projects", CreateProject)
            .RequireAuthorization("Admin");

        app.MapPut("/projects/{id:guid}", UpdateProject)
            .RequireAuthorization("Admin");

        app.MapDelete("/projects/{id:guid}", DeleteProject)
            .RequireAuthorization("Admin");

        app.MapGet("/projects/reports", GetTasksPerProject)
            .RequireAuthorization("Admin");
    }

    private static async Task<IResult> GetTasksPerProject(IProjectService projectService)
    {
        var report = await projectService.GetTasksPerProjectAsync().ConfigureAwait(false);

        return Results.Ok(report);
    }

    private static async Task<IResult> GetAllProjects(IProjectService projectService)
    {
        var projects = await projectService.GetAllProjectsAsync().ConfigureAwait(false);

        return Results.Ok(projects);
    }

    private static async Task<IResult> GetProjectById(Guid id, IProjectService projectService)
    {
        var project = await projectService.GetProjectByIdAsync(id).ConfigureAwait(false);

        return project == null ? Results.NotFound() : Results.Ok(project);
    }

    private static async Task<IResult> CreateProject(ProjectDTO projectDto, IProjectService projectService)
    {
        var createdProject = await projectService.CreateProjectAsync(projectDto).ConfigureAwait(false);

        return Results.Created(new Uri($"/projects/{createdProject.Id}", UriKind.Relative), createdProject);
    }

    private static async Task<IResult> UpdateProject(Guid id, UpdateProjectDTO projectDto, IProjectService projectService)
    {
        var updatedProject = await projectService.UpdateProjectAsync(id, projectDto).ConfigureAwait(false);

        return updatedProject == null ? Results.NotFound() : Results.Ok(updatedProject);
    }

    private static async Task<IResult> DeleteProject(Guid id, IProjectService projectService)
    {
        var success = await projectService.DeleteProjectAsync(id).ConfigureAwait(false);

        return success ? Results.NoContent() : Results.NotFound();
    }
}

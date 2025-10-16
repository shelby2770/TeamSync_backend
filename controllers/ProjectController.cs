using Microsoft.AspNetCore.Mvc;
using TeamSyncB.models;
using TeamSyncB.services;

namespace TeamSyncB.controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectService.GetAllProjects();
            return Ok(projects);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProject(Guid projectId)
        {
            var project = await _projectService.GetProjectById(projectId);
            if (project == null) return NotFound();
            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newProject = await _projectService.CreateProject(project);
            return Ok(newProject);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserProjects(Guid userId)
        {
            var projects = await _projectService.GetUserProjects(userId);
            return Ok(projects);
        }
    }
}

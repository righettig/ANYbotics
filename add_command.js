const fs = require('fs');
const path = require('path');

// Get the command name from the command line arguments
const COMMAND_NAME = process.argv[2];
if (!COMMAND_NAME) {
    console.error("Please provide a command name as an argument.");
    process.exit(1);
}

const COMMAND_NAME_CAMEL_CASE = COMMAND_NAME.charAt(0).toLowerCase() + COMMAND_NAME.slice(1);

// File paths
const protoFilePath = path.join(__dirname, 'anybotics-anymal-common', 'Protos', 'anymal.proto');
const commandFilePath = path.join(__dirname, 'anybotics-anymal-api', 'Commands', `${COMMAND_NAME}Command.cs`);
const commandHandlerFilePath = path.join(__dirname, 'anybotics-anymal-api', 'Commands', 'CommandHandlers', `${COMMAND_NAME}CommandHandler.cs`);
const controllerFilePath = path.join(__dirname, 'anybotics-anymal-api', 'Controllers', `${COMMAND_NAME}Controller.cs`);
const anymalServiceFilePath = path.join(__dirname, 'anybotics-anymal-api', 'Services', `AnymalService.${COMMAND_NAME}.cs`);
const commandProcessorFilePath = path.join(__dirname, 'anybotics-anymal', 'CommandProcessors', `${COMMAND_NAME}CommandProcessor.cs`);
const agentServiceFilePath = path.join(__dirname, 'anybotics-workforce-ng', 'src', 'app', 'services', 'agent.service.ts');

// Helper functions
function addProtoMessage() {
    const messageDefinition = `
message ${COMMAND_NAME}Request {
  string id = 1;
}

message ${COMMAND_NAME}Response {
  bool success = 1;
}\n`;

    fs.appendFileSync(protoFilePath, messageDefinition, 'utf8');
    console.log(`Proto message added for ${COMMAND_NAME}`);
}

function createCommandFile() {
    const commandContent = `
namespace anybotics_anymal_api.Commands;

public class ${COMMAND_NAME}Command(string agentId, string initiatedBy) : CommandBase(agentId, initiatedBy)
{
    public override string ToString() => "${COMMAND_NAME}";
}\n`;

    fs.writeFileSync(commandFilePath, commandContent, 'utf8');
    console.log(`Command file created for ${COMMAND_NAME}`);
}

function createCommandHandlerFile() {
    const commandHandlerContent = `
using AnymalGrpc;
using AnymalService = anybotics_anymal_api.Services.AnymalService;

namespace anybotics_anymal_api.Commands.CommandHandlers;

public class ${COMMAND_NAME}CommandHandler(AnymalService anymalService) : CommandHandlerBase<${COMMAND_NAME}Command>(anymalService)
{
    public override async Task<UpdateResponse> HandleAsync(${COMMAND_NAME}Command command)
    {
        return await anymalService.${COMMAND_NAME}Async(command.AgentId);
    }
}\n`;

    fs.writeFileSync(commandHandlerFilePath, commandHandlerContent, 'utf8');
    console.log(`Command handler file created for ${COMMAND_NAME}`);
}

function createControllerFile() {
    const controllerContent = `
using anybotics_anymal_api.Commands;
using anybotics_anymal_api.CustomAttributes;
using Microsoft.AspNetCore.Mvc;

namespace anybotics_anymal_api.Controllers;

[ApiController]
[Route("anymal/${COMMAND_NAME_CAMEL_CASE}")]
public class ${COMMAND_NAME}Controller(ICommandBus commandBus) : BaseAnymalCommandController(commandBus)
{
    [HttpPost]
    [Deny("guest")]
    public async Task<IActionResult> ${COMMAND_NAME}([FromBody] string id)
    {
        var validationResult = ValidateId(id);
        if (validationResult != null)
        {
            return validationResult;
        }

        var result = await commandBus.SendAsync(new ${COMMAND_NAME}Command(id, UserUid));
        return Ok(result);
    }
}\n`;

    fs.writeFileSync(controllerFilePath, controllerContent, 'utf8');
    console.log(`Controller file created for ${COMMAND_NAME}`);
}

function createAnymalServiceFile() {
    const anymalServiceContent = `
using AnymalGrpc;

namespace anybotics_anymal_api.Services;

public partial class AnymalService
{
    public Task<UpdateResponse> ${COMMAND_NAME}Async(string id)
        => PerformAgentActionAsync(id, async agentClient =>
        {
            if (agentClient.Agent.Status == AnymalGrpc.Status.Offline)
            {
                throw new InvalidOperationException("Agent is Offline. ${COMMAND_NAME} requests are ignored.");
            }

            var @event = new Command { Id = id, CommandId = "${COMMAND_NAME}" };
            await agentClient.CommandStream?.WriteAsync(@event);
        },
        $"Performing ${COMMAND_NAME} agent {id}.", "Agent not found.");
}\n`;

    fs.writeFileSync(anymalServiceFilePath, anymalServiceContent, 'utf8');
    console.log(`AnymalService file created for ${COMMAND_NAME}`);
}

function createCommandProcessorFile() {
    const commandProcessorContent = `
using AnymalGrpc;

namespace anybotics_anymal.CommandProcessors;

public class ${COMMAND_NAME}CommandProcessor() : BaseCommandProcessor("${COMMAND_NAME}")
{
    public override void PerformCommand(Agent agent, Command response)
    {
        // placeholder
    }
}\n`;

    fs.writeFileSync(commandProcessorFilePath, commandProcessorContent, 'utf8');
    console.log(`Command processor created for ${COMMAND_NAME}`);
}

function updateAgentServiceFile() {
    const agentServiceContent = `
  async ${COMMAND_NAME_CAMEL_CASE}(id: string): Promise<void> {
    const url = \`\${this.baseApiUrl}/${COMMAND_NAME_CAMEL_CASE}\`;
    await this.performAction(url, id);
  }\n`;

    fs.appendFileSync(agentServiceFilePath, agentServiceContent, 'utf8');
    console.log(`Agent service updated for ${COMMAND_NAME}`);
}

// Main function
function main() {
    addProtoMessage();
    createCommandFile();
    createCommandHandlerFile();
    createControllerFile();
    createAnymalServiceFile();
    createCommandProcessorFile();
    updateAgentServiceFile();
}

// Execute script
main();

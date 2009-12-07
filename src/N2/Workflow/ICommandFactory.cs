﻿using System;
namespace N2.Workflow
{
    /// <summary>
    /// Provides and executies commands used to change the state of content items.
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>Gets the command to save and preview changes.</summary>
        /// <param name="context">The command context used to determine which command to return.</param>
        /// <returns>A command that when executed will publish an item.</returns>
        CommandBase<CommandContext> GetPreviewCommand(CommandContext context);
        
        /// <summary>Gets the command used to publish an item.</summary>
        /// <param name="context">The command context used to determine which command to return.</param>
        /// <returns>A command that when executed will publish an item.</returns>
        CommandBase<CommandContext> GetPublishCommand(CommandContext context);
        
        /// <summary>Gets the command to save changes to an item without leaving the editing interface.</summary>
        /// <param name="context">The command context used to determine which command to return.</param>
        /// <returns>A command that when executed will save an item.</returns>
        CommandBase<CommandContext> GetSaveCommand(CommandContext context);
    }
}
using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Enumerations;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Models.Security;

namespace AndcultureCode.CSharp.Core.Interfaces.Conductors
{
    /// <summary>
    /// Determines permission leveraging access control lists
    /// </summary>
    public interface IPermissionConductor
    {
        /// <summary>
        /// Get a list of Access Rules for a given resource/verb pair.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="verb"></param>
        /// <returns></returns>
        IResult<List<AccessRule>> GetAcls(string resource, string verb);

        /// <summary>
        /// Determine if a given Role is allowed to perform a certain action, given as a
        /// resource/verb pair.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="verb"></param>
        /// <param name="subject">The ID for the Role that we're checking permissions for.</param>
        /// <returns></returns>
        IResult<bool> IsAllowed(string resource, string verb, string subject);

        /// <summary>
        /// Determine if a given Role is allowed to perform either:
        ///   - at least one action in a list of actions (op = OR)
        ///    or
        ///   - all of the actions in a list of actions (op = AND)
        /// </summary>
        /// <param name="op">A LogicalOperator value that dictates which validation method to use.</param>
        /// <param name="resourceVerbs">A list of actions as ResourceVerb objects.</param>
        /// <param name="subject">The ID for the Role that we're checking permissions for.</param>
        /// <returns></returns>
        IResult<bool> IsAllowed(LogicalOperator op, IEnumerable<ResourceVerb> resourceVerbs, string subject);

        /// <summary>
        /// Determine if a given Role is allowed to perform either:
        ///   - at least one action whose resource starts with the given prefix (op = OR)
        ///    or
        ///   - all of the actions that start with the given prefix (op = AND)
        /// </summary>
        /// <param name="op">A LogicalOperator value that dictates which validation method to use.</param>
        /// <param name="resourcePrefix">The resource prefix that we use to get the actions to check permissions on.</param>
        /// <param name="subject">The ID for the Role that we're checking permissions for.</param>
        /// <returns></returns>
        IResult<bool> IsAllowed(LogicalOperator op, string resourcePrefix, string subject);
    }
}

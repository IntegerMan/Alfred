using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

using Assisticant;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Common;
using MattEland.Common.Annotations;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     The view model locator that helps views find appropriate view models.
    /// </summary>
    public sealed class ViewModelLocator : ViewModelLocatorBase
    {
        /// <summary>
        ///     The default number of MFDs present.
        /// </summary>
        private const int DefaultMFDCount = 6;

        /// <summary>
        ///     A mapping of model types to its view model.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly Dictionary<Type, Type> _modelToViewModelMapping;

        [NotNull]
        private readonly Workspace _workspace;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ViewModelLocator"/> class.
        /// </summary>
        [UsedImplicitly]
        public ViewModelLocator() : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ViewModelLocator"/> class.
        /// </summary>
        /// <param name="container"> The container. </param>
        [UsedImplicitly]
        public ViewModelLocator([CanBeNull] IAlfredContainer container)
        {
            Container = container ?? new AlfredContainer(CommonProvider.Container);

            // Set up mappings for automatic creation of view models.
            _modelToViewModelMapping = new Dictionary<Type, Type>();
            BuildViewModelMappings();

            _workspace = LoadWorkspace();
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IAlfredContainer Container { get; }

        /// <summary>
        ///     Gets the main view model.
        /// </summary>
        /// <value>
        ///     The main view model.
        /// </value>
        [NotNull]
        public object Main
        {
            get
            {
                var vm = ViewModel(() => new MainViewModel(this, _workspace));

                return vm;
            }
        }

        [NotNull]
        internal object ViewModelFor([NotNull] object model)
        {
            Contract.Requires(model != null);

            // Use attribute-based lookup to create the view model
            var vm = BuildViewModelForModel(model);

            // Wrap it into an Assisticant wrapper
            return ForView.Wrap(vm);
        }

        /// <summary>
        ///     Configures a multifunction display.
        /// </summary>
        /// <param name="mfd"> The mfd. </param>
        /// <param name="index"> The zero-based index of the multifunction display. </param>
        private static void ConfigureDesignMFD([NotNull] MultifunctionDisplay mfd, int index)
        {
            mfd.Name = string.Format("Design MFD {0}", index + 1);
        }

        /// <summary>
        ///     Configures a multifunction display.
        /// </summary>
        /// <param name="mfd"> The mfd. </param>
        /// <param name="index"> The zero-based index of the multifunction display. </param>
        private static void ConfigureMFD([NotNull] MultifunctionDisplay mfd, int index)
        {
            mfd.Name = string.Format("MFD {0}", index + 1);
        }

        /// <summary>
        ///     Builds a view model for the specified model.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns>
        ///     A ViewModel.
        /// </returns>
        private object BuildViewModelForModel([NotNull] object model)
        {

            var modelType = model.GetType();
            var vmType = _modelToViewModelMapping[modelType];

            if (vmType == null)
            {
                var message = string.Format("No type mapping found for model type {0}", modelType.Name);
                throw new NotSupportedException(message);
            }

            //! Create the instance passing in the model to its constructor
            return Container.CreateInstance(vmType, model);
        }

        /// <summary>
        ///     Builds view model mappings as needed.
        /// </summary>
        private void BuildViewModelMappings()
        {
            //- Early exit if we've been here before
            if (_modelToViewModelMapping.Any()) return;

            var attributeType = typeof(ViewModelForAttribute);

            // Look over all assemblies in scope

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in allAssemblies)
            {
                // Grab types with the mapping attribute
                var types = assembly.GetTypesInAssemblyWithAttribute(attributeType, true);

                // Map the type to the type it should represent
                foreach (var type in types)
                {
                    var atr = type.GetCustomAttribute(attributeType) as ViewModelForAttribute;

                    if (atr == null) continue;

                    // Set the mapping
                    _modelToViewModelMapping[atr.Model] = type;
                }
            }
        }

        /// <summary>
        ///     Loads the workspace.
        /// </summary>
        /// <returns>
        ///     The workspace.
        /// </returns>
        [NotNull]
        private Workspace LoadWorkspace()
        {
            // Create the workspace
            var workspace = new Workspace(Container);

            // Add all MFDs to the workspace
            for (int index = 0; index < DefaultMFDCount; index++)
            {
                var mfd = workspace.NewMFD();

                if (DesignMode)
                {
                    ConfigureDesignMFD(mfd, index);
                }
                else
                {
                    ConfigureMFD(mfd, index);
                }
            }

            // Automatically make the first MFD the sensor of interest.
            workspace.SelectedMFD = workspace.MFDs.First();

            return workspace;
        }
    }

}

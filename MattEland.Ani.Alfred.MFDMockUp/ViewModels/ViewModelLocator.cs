using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Assisticant;

using MattEland.Ani.Alfred.MFDMockUp.Models;
using MattEland.Ani.Alfred.MFDMockUp.Models.Screens;
using MattEland.Common;
using MattEland.Common.Annotations;
using MattEland.Ani.Alfred.PresentationCommon.Helpers;
using MattEland.Common.Providers;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    /// <summary>
    ///     The view model locator that helps views find appropriate view models.
    /// </summary>
    public sealed class ViewModelLocator : ViewModelLocatorBase, IHasContainer<IObjectContainer>
    {
        /// <summary>
        ///     The default number of MFDs present.
        /// </summary>
        private const int DefaultMFDCount = 6;

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
        public ViewModelLocator([CanBeNull] IObjectContainer container)
        {
            Container = container ?? CommonProvider.Container;

            // Register a single random number generator for any interested parties
            Container.RegisterProvidedInstance(typeof(Random), new Random());

            _modelToViewModelMapping = new Dictionary<Type, Type>();
            _workspace = LoadWorkspace();
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        [NotNull]
        public IObjectContainer Container { get; }

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

            return workspace;
        }

        /// <summary>
        ///     Configures a multifunction display.
        /// </summary>
        /// <param name="mfd"> The mfd. </param>
        /// <param name="index"> The zero-based index of the multifunction display. </param>
        private static void ConfigureMFD([NotNull] MultifunctionDisplay mfd, int index)
        {
            mfd.Name = string.Format("MFD {0}", index + 1);

            mfd.IsSensorOfInterest = index == 0;
        }

        /// <summary>
        ///     Configures a multifunction display.
        /// </summary>
        /// <param name="mfd"> The mfd. </param>
        /// <param name="index"> The zero-based index of the multifunction display. </param>
        private static void ConfigureDesignMFD([NotNull] MultifunctionDisplay mfd, int index)
        {
            mfd.Name = string.Format("Design MFD {0}", index + 1);

            mfd.IsSensorOfInterest = index == 0;
        }

        [NotNull]
        internal object ViewModelFor(ScreenModel screenModel)
        {
            return ViewModel(() => BuildViewModelForModel(screenModel)); ;
        }

        /// <summary>
        ///     A mapping of model types to its view model.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly Dictionary<Type, Type> _modelToViewModelMapping;

        /// <summary>
        ///     Builds a view model for the specified model.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns>
        ///     A ViewModel.
        /// </returns>
        private object BuildViewModelForModel([NotNull] object model)
        {
            BuildViewModelMappingsAsNeeded();

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
        private void BuildViewModelMappingsAsNeeded()
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
    }

}

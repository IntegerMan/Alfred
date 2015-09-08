#Subsystem Registration

When Subsystems register, they will need to effectively register themselves as well as a registry of
all of their children and capabilities. This includes:

- Chat Providers
- Command Handlers
- Pages (root and otherwise)
- Search Providers

At no point after Subsystem registration can this list change. Items can become disabled and enabled,
but new capabilities cannot be added.

Subsystem registration will still occur the same way from a code perspective where the call is similar to:

	var system = new MySubsystem();
	Alfred.Register(system);

The assumption is that MySubsystem is fully configured at this point and changes in functionality will be
met with exceptions, non-functional features, or other unanticipated behavior.

When a new subsystem is registered Alfred will query it to see if it implements the following interfaces:

- IChatProvider
- ICommandHandler
- IPageProvider
- ISearchProvider

If a Subsystem matches any of these interfaces, Alfred will use the supported interface to register its
provided functionality. These methods will not be called again except for during registration.

Once a Subsystem has its capabilities registered with Alfred, Alfred will use those established provided
objects to handle application events and provide application features. For example, if a Subsystem is an
ISearchProvider, its search providers will be called upon to handle searches and provide results.

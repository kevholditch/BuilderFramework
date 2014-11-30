BuilderFramework
================

A simple lightweight framework that allows you to construct steps that can be committed and rolled back.

## Why

Several times now I have written very similar code to provide a framework for committing and then rolling back steps.  When all that changes from project to project is what you are doing in your step.  This project aims to provide an easy to use framework that will commit steps in dependency order and allow you to roll them back again.  Making it a perfect helper framwork for writing tests.

## How

To implement a step simply implement the IBuildStep interface:

    public interface IBuildStep
    {
        void Commit();
        void Rollback();
    }
    
For example:

    public class ExampleStep : IBuildStep
    {
        public void Commit()
        {
            Console.WriteLine("Commiting...");
        }
        
        public void Rollback()
        {
            Console.WriteLine("Rolling back...");
        }
    }
    
Once you have done this add your step to the builder using With:

    var builder = new Builder().With(new ExampleStep());
    
The builder also accepts a function that builds a step.  Allowing you to write a fluent builder to build your step. For example:

    public class ExampleStepBuilder
    {
        private ExampleStep _exampleStep;
        
        public ExampleStepBuilder WithParameter(int value)
        {
            _exampleStep.Parameter = value;
        }
        
        public ExampleStep Build()
        {
            return _exampleStep;
        }
    }
    
    
    var builder = new Builder().With<ExampleStepBuilder>(e => e.WithParamter(42).Build());
    
The builder keeps returning itself so you can keep chainging calls:

    var builder = new Builder().With(new ExampleStep()).With(new ExampleStep());
    

    
Now to commit all of the steps added:

    builder.Commit();
    
To rollback:
    `builder.Rollback();
    
You can specify that one step depends on another using the DependsOnAttribute.  The builder will automatically execute the steps in dependency order (or throw a circular reference exception if you have a circular reference).

   `[DependsOn(typeof(ExampleBuildStep)] public class MyStepA : IBuildStep
   
If you use the depends on attribute the steps will be rolled back in the opposite order to which they are built in.  Allowing you to use the builder to commit changes to a database, run a test and then roll them back.  Note that the builder doesn't check to make sure you have actually included the dependent step, it will just ensure that the dependent step runs before this step (if its added to the builder).
   
    
    




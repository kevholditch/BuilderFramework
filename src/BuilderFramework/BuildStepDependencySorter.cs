using System;
using System.Collections.Generic;
using System.Linq;
using BuilderFramework.Exceptions;

namespace BuilderFramework
{
    public class BuildStepDependencySorter : IBuildStepDependencySorter
    {
        private class Node
        {
            public Node(IBuildStep buildStep)
            {
                BuildStep = buildStep;
                IncomingEdges = new List<Edge>();
                OutgoingEdges = new List<Edge>();
            }

            public IBuildStep BuildStep { get; private set; }
            public List<Edge> IncomingEdges { get; private set; }
            public List<Edge> OutgoingEdges { get; private set; }
        }

        private class Edge
        {
            public Edge(Node sourceNode, Node destinationNode)
            {
                SourceNode = sourceNode;
                DestinationNode = destinationNode;
            }

            public Node SourceNode { get; private set; }
            public Node DestinationNode { get; private set; }

            public void Remove()
            {
                SourceNode.OutgoingEdges.Remove(this);
                DestinationNode.IncomingEdges.Remove(this);
            }
        }

        public IEnumerable<IBuildStep> Sort(IEnumerable<IBuildStep> buildSteps)
        {
            List<Node> nodeGraph = buildSteps.Select(buildStep => new Node(buildStep)).ToList();

            foreach (var node in nodeGraph)
            {
                var depends = (DependsOnAttribute[])Attribute.GetCustomAttributes(node.BuildStep.GetType(), typeof(DependsOnAttribute));
                var dependNodes = nodeGraph.Where(n => depends.Any(d => d.DependedOnStep == n.BuildStep.GetType()));

                var edges = dependNodes.Select(n => new Edge(node, n)).ToArray();
                node.OutgoingEdges.AddRange(edges);

                foreach (var edge in edges)
                    edge.DestinationNode.IncomingEdges.Add(edge);
            }

            var result = new Stack<Node>();
            var sourceNodes = new Stack<Node>(nodeGraph.Where(n => !n.IncomingEdges.Any()));
            while (sourceNodes.Count > 0)
            {
                var sourceNode = sourceNodes.Pop();
                result.Push(sourceNode);

                for (int i = sourceNode.OutgoingEdges.Count - 1; i >= 0; i--)
                {
                    var edge = sourceNode.OutgoingEdges[i];
                    edge.Remove();

                    if (!edge.DestinationNode.IncomingEdges.Any())
                        sourceNodes.Push(edge.DestinationNode);
                }
            }

            if (nodeGraph.SelectMany(n => n.IncomingEdges).Any())
                throw new CircularDependencyException();

            return result.Select(n => n.BuildStep);
        }


    }
}
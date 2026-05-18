using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
public class NodeView : GraphView
{
        public StoryLinker creator = null;
        public StoryInfo Info = null;
        public NodeView(StoryLinker creatorWnd)
        {
            creator = creatorWnd;
            SetInterface();
            graphViewChanged += WhenViewChanged;
        }
        public Type GetGuidType(BaseInfo baseInfo)
        {
            var port = GetPortByGuid(baseInfo.inPort.guid);
            if (port?.node == null)
            {
                Debug.LogWarning($"找不到 GUID 为 {baseInfo.inPort.guid} 的端口");
                return null;
            }
            return port.node.GetType();
        }
        public void Fresh(StoryInfo info)
        {
            NodeEdgeClear();
            Info = info;
            AddElement(new ToolNode(Info));
            Add(new MiniMap());
            Debug.Log($"Fresh: Info.list 中有 {info.list?.Count ?? -1} 个节点");
            LoadNodeAndEdge(Info);
        }
        private void LoadNodeAndEdge(StoryInfo info)
        {
            var start = new StartNode(Info.startInfo);
            AddElement(start);
            CreateNode(info.list);
            if (StoryLinker.Config.DebugMode)
            {
                Debug.Log("In DebugMode,Skip LinkPorts");
                return;
            }
            LinkPorts(Info.startInfo);
            foreach (var nodeInfo in info.list)
            {
                LinkPorts(nodeInfo);
            }
        }
        private void LinkPorts(BaseInfo info)
        {
            var outPort = GetPortByGuid(info.outPort.guid);
            foreach (var nextGuid in info.outPort.nextGuids)
            {
                var inPort = GetPortByGuid(nextGuid);
                if (outPort != null && inPort != null)
                    AddElement(outPort.ConnectTo(inPort));
            }
        }
        private void CreateNode<T>(List<T> list) where T : BaseInfo
        {
            foreach (var info in list)
            {
                switch (info)
                {
                    case DialogueInfo dialogue:
                        AddElement(new DialogueNode().Init(dialogue));
                        break;
                    case OptionInfo option:
                        AddElement(new OptionNode().Init(option));
                        break;
                    case EndInfo end:
                        AddElement(new EndNode().Init(end));
                        break;
                    case NoteInfo note:
                        AddElement(new NoteNode().Init(note));
                        break;
                }
            }
        }
        private void NodeEdgeClear()
        {
            // 先复制集合，避免迭代时修改导致异常
            var edgesToRemove = edges.ToList();
            var nodesToRemove = nodes.ToList();
            foreach (var edge in edgesToRemove)
            {
                RemoveElement(edge);
            }
            foreach (var node in nodesToRemove)
            {
                RemoveElement(node);
            }
        }
        private void SetInterface()
        {
            style.flexGrow = 1;

            this.AddManipulator(new SelectionDragger());  // 选择拖动器：拖动选择区域
            this.AddManipulator(new ContentDragger()); // 矩形选择器：拖动选择区域

            SetupZoom(0.25f, 1.25f);

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
            styleSheets.Add(StoryLinker.Config.graphViewBG);
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return (from nap in ports.ToList()
                    where nap.direction != startPort.direction && nap.node != startPort.node
                    select nap).ToList();
        }
        private GraphViewChange WhenViewChanged(GraphViewChange change)
        {
            bool needSave = OnRemove(change);
            needSave |= OnMove(change);
            needSave |= OnCreateEdge(change);

            if (needSave && Info != null)
            {
                Info.Save();
            }
            return change;
        }
        private static bool OnCreateEdge(GraphViewChange change)
        {
            if (change.edgesToCreate == null) return false;
            //edge的output指的是node的output
            foreach (Edge edge in change.edgesToCreate)
            {
                var thisInfo = edge.output.userData as BaseInfo;
                var nextInfo = edge.input.userData as BaseInfo;
                thisInfo.LinkTo(nextInfo.inPort.guid);
            }
            return true;
        }
        private static bool OnMove(GraphViewChange change)
        {
            if (change.movedElements == null) return false;
            foreach (GraphElement ele in change.movedElements)
            {
                if (ele is InfoNode node)
                    node.info.UpdatePos(node.Pos);
            }
            return true;
        }
        private bool OnRemove(GraphViewChange change)
        {
            if (change.elementsToRemove == null) return false;
            foreach (GraphElement ele in change.elementsToRemove)
            {
                if (ele is InfoNode node)
                {
                    Info.Remove(node.info);
                }
                if (ele is Edge edge)
                {
                    var thisInfo = edge.output.userData as BaseInfo;
                    var nextInfo = edge.input.userData as BaseInfo;
                    thisInfo.DisConnect(nextInfo.inPort.guid);
                }
            }
            return true;
        }
    }
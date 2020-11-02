using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
namespace Bll.Logic
{

    // A ure to represent a node in adjacency list 
    public class AdjListNode
    {
        public int dest;
        public int weight;
        public AdjListNode next;

        public AdjListNode()
        {
        }
        // A utility function to create a new adjacency list node 
        public AdjListNode(int dest, int weight)
        {
            this.dest = dest;
            this.weight = weight;
            this.next = null;
        }
    }

    // A ure to represent an adjacency list 
    public class AdjList
    {
        // pointer to head node of list 
        public AdjListNode head { get; set; }
        public AdjList() { }
        public int SetHead() { head = null; return 1; }
    }

    // A ure to represent a graph. A graph is an array of adjacency lists. 
    // Size of array will be V (number of vertices in graph) 
    public class Graph
    {
        public int v;
        public AdjList[] array;
        public Graph() { }
        // A utility function that creates a graph of V vertices 
        public Graph(int v)
        {
            this.v = v;
            // Create an array of adjacency lists.  Size of array will be V 
            this.array = new AdjList[v];
            // Initialize each adjacency list as empty by making head as NULL 
            for (int i = 0; i < v; ++i)
                this.array[i] = new AdjList() { head = null };
        }

        public void addEdge(int src, int dest, int weight)
        {
            // Add an edge from src to dest.  A new node is added to the adjacency 
            // list of src.  The node is added at the beginning 
            AdjListNode newNode = new AdjListNode(dest, weight);
            newNode.next = this.array[src].head;
            this.array[src].head = newNode;

            // Since graph is undirected, add an edge from dest to src also 
            newNode = new AdjListNode(src, weight);
            newNode.next = this.array[dest].head;
            this.array[dest].head = newNode;
        }
    }
    #region ערימת מינימום
    // ure to represent a min heap node 
    public class MinHeapNode
    {
        public int v;
        public int dist;

        public MinHeapNode(int v, int dist)
        {
            this.v = v;
            this.dist = dist;
        }
    }

    // ure to represent a min heap 
    public class MinHeap
    {
        public int size;      // Number of heap nodes present currently 
        public int capacity;  // Capacity of min heap 
        public int[] pos;     // This is needed for decreaseKey() 
        public MinHeapNode[] array;

        public MinHeap(int capacity)
        {
            this.pos = new int[capacity];
            this.size = 0;
            this.capacity = capacity;
            this.array = new MinHeapNode[capacity];
        }
        public void swapMinHeapNodeProblem(MinHeapNode a, MinHeapNode b, out MinHeapNode aa, out MinHeapNode bb)
        {
            aa = b;
            bb = a;
        }

        public void minHeapify(int idx)
        {
            int smallest, left, right;
            smallest = idx;
            left = 2 * idx + 1;
            right = 2 * idx + 2;

            if (left < this.size &&
               this.array[left].dist < this.array[smallest].dist)
                smallest = left;

            if (right < this.size &&
                this.array[right].dist < this.array[smallest].dist)
                smallest = right;

            if (smallest != idx)
            {
                // The nodes to be swapped in min heap 
                MinHeapNode smallestNode = this.array[smallest];
                MinHeapNode idxNode = this.array[idx];

                // Swap positions 
                this.pos[smallestNode.v] = idx;
                this.pos[idxNode.v] = smallest;

                // Swap nodes 
                MinHeapNode temp = this.array[smallest];
                this.array[smallest] = this.array[idx];
                this.array[idx] = temp;
                minHeapify(smallest);
            }
        }

        // A utility function to check if the given minHeap is ampty or not 
        public bool isEmpty()
        {
            return this.size == 0;
        }

        // Standard function to extract minimum node from heap 
        public MinHeapNode extractMin()
        {
            if (this.isEmpty())
                return null;

            // Store the root node 
            MinHeapNode root = this.array[0];

            // Replace root node with last node 
            MinHeapNode lastNode = this.array[this.size - 1];
            this.array[0] = lastNode;

            // Update position of last node 
            this.pos[root.v] = this.size - 1;
            this.pos[lastNode.v] = 0;

            // Reduce heap size and heapify root 
            --this.size;
            minHeapify(0);

            return root;
        }

        // Function to decreasy dist value of a given vertex v. This function 
        // uses pos[] of min heap to get the current index of node in min heap 
        public void decreaseKey(int v, int dist)
        {
            // Get the index of v in  heap array 
            int i = this.pos[v];

            // Get the node and update its dist value 
            this.array[i].dist = dist;

            // Travel up while the complete tree is not hepified. 
            // This is a O(Logn) loop 
            while (i != 0 && this.array[i].dist < this.array[(i - 1) / 2].dist)
            {
                // Swap this node with its parent 
                this.pos[this.array[i].v] = (i - 1) / 2;
                this.pos[this.array[(i - 1) / 2].v] = i;
                //swap( this.array[i], this.array[(i - 1) / 2]);
                MinHeapNode temp;
                //swapMinHeapNode(this.array[i], this.array[(i - 1) / 2], out   aa, out  bb);
                temp = this.array[i];
                this.array[i] = this.array[(i - 1) / 2];
                this.array[(i - 1) / 2] = temp;
                // move to parent index 
                i = (i - 1) / 2;
            }
        }

        // A utility function to check if a given vertex 
        // 'v' is in min heap or not 
        public bool isInMinHeap(int v)
        {
            if (this.pos[v] < this.size)
                return true;
            return false;
        }

    }

    // A utility function to create a new Min Heap Node 


    // A utility function to create a Min Heap 


    // A utility function to swap two nodes of min heap. Needed for min heapify 


    // A standard function to heapify at given idx 
    // This function also updates position of nodes when they are swapped. 
    // Position is needed for decreaseKey() 

    #endregion


    public class DijkstraFunction
    {

        //public static void printArr(Cell[] dist, int n)
        //{
        //    Console.WriteLine("\n\n Vertex \t\t  Distance from Source \t\t parent \n");
        //    for (int i = 0; i < n; ++i)
        //        Console.WriteLine("\n" + i + "  \t\t " + dist[i].distance + " \t\t" + dist[i].Parent);
        //}

        // The main function that calulates distances of shortest paths from src to all 
        // vertices. It is a O(ELogV) function 
        public static Cell[] Dijkstra(Graph graph, int src)
        {
            int vv = graph.v;// Get the number of vertices in graph 
            Cell[] dist = new Cell[vv];      // dist values used to pick minimum weight edge in cut 
            for (int i = 0; i < vv; i++)
            {
                dist[i] = new Cell();
                dist[i].i = src;
                dist[i].j = i;
                dist[i].Parent = 0;
                dist[i].distance = int.MaxValue;
            }
            // minHeap represents set E 
            MinHeap minHeap = new MinHeap(vv);

            // Initialize min heap with all vertices. dist value of all vertices  
            for (int v = 0; v < vv; ++v)
            {
                dist[v].distance = int.MaxValue;
                minHeap.array[v] = new MinHeapNode(v, dist[v].distance);
                minHeap.pos[v] = v;
            }
            // Make dist value of src vertex as 0 so that it is extracted first 
            minHeap.array[src] = new MinHeapNode(src, dist[src].distance);
            minHeap.pos[src] = src;
            dist[src].distance = 0;
            minHeap.decreaseKey(src, dist[src].distance);
            // Initially size of min heap is equal to V 
            minHeap.size = vv;

            // In the followin loop, min heap contains all nodes 
            // whose shortest distance is not yet finalized. 
            while (!minHeap.isEmpty())
            {
                // Extract the vertex with minimum distance value 
                MinHeapNode minHeapNode = minHeap.extractMin();
                int u = minHeapNode.v; // Store the extracted vertex number 

                // Traverse through all adjacent vertices of u (the extracted 
                // vertex) and update their distance values 
                AdjListNode pCrawl = graph.array[u].head;
                while (pCrawl != null)
                {
                    int v = pCrawl.dest;

                    // If shortest distance to v is not finalized yet, and distance to v 
                    // through u is less than its previously calculated distance 
                    if (minHeap.isInMinHeap(v) && dist[u].distance != int.MaxValue &&
                                                  pCrawl.weight + dist[u].distance < dist[v].distance)
                    {
                        dist[v].distance = dist[u].distance + pCrawl.weight;
                        dist[v].Parent = u;
                        // update distance value in min heap also 
                        minHeap.decreaseKey(v, dist[v].distance);
                    }
                    pCrawl = pCrawl.next;
                }
            }

            // print the calculated shortest distances 
            //printArr(dist, vv);
            return dist;
        }
        // A utility function used to print the solution 

        // Driver program to test above functions 
        //public static Cell[] DOmain()
        //{
        //    // create the graph given in above fugure 
        //    int V = 9;
        //    Graph graph = new Graph(V);
        //    graph.addEdge(0, 1, 4);
        //    graph.addEdge(0, 7, 8);
        //    graph.addEdge(1, 2, 8);
        //    graph.addEdge(1, 7, 11);
        //    graph.addEdge(2, 3, 7);
        //    graph.addEdge(2, 8, 2);
        //    graph.addEdge(2, 5, 4);
        //    graph.addEdge(3, 4, 9);
        //    graph.addEdge(3, 5, 14);
        //    graph.addEdge(4, 5, 10);
        //    graph.addEdge(5, 6, 2);
        //    graph.addEdge(6, 7, 1);
        //    graph.addEdge(6, 8, 6);
        //    graph.addEdge(7, 8, 7);

        //    Dijkstra(graph, 0);
        //    ShopWayEntities shopWay = new ShopWayEntities();
        //    Graph graph2 = ToAdjList(shopWay.Getaways.ToList(), shopWay.Connections.ToList());
        //    return Dijkstra(graph2, 1);

        //}
        public static Cell[,] ComputeDikjstra(List<GetawayProcI_Result> getaways, List<Connection> connections)
        {
            int numGetaways = getaways.Count() + 1;
            Graph graph = ToAdjList(getaways, connections);
            //מטריצת המרחקים שתוחזר 
            Cell[,] mat = new Cell[numGetaways, numGetaways];
            //משתנה עזר המקבל תוצאת דייקסטרה ומועתק למטריצה
            Cell[] arr = new Cell[numGetaways];
            foreach (var item in getaways)
            {
                arr = Dijkstra(graph, Convert.ToInt32( item.I));
                for (int j = 1; j < numGetaways; j++)
                    mat[Convert.ToInt32(item.I), j] = arr[j];      
            }
            return mat;
        }
        public static Graph ToAdjList(List<GetawayProcI_Result> points, List<Connection> connections)
        {
            
            //המרת קשתות וקודקודים לרשימת סמיכויות
            List<Connection> connectionsGetaway = connections.Where(x => x.TypeDest == Convert.ToInt32(eTypeConnection.getaway)).ToList();
            int V = points.Count() + 1;
            Graph graph = new Graph(V);
            int i = 0;
            int num = points[0].Code;
            foreach (var item in connectionsGetaway)
            {
                int src = Convert.ToInt32(item.CodeSource);
                int dest = Convert.ToInt32(item.CodeDest);
                int Isrc=GetawayProcI_Result.GetI(points,src);
                int Idest = GetawayProcI_Result.GetI(points, dest);
                graph.addEdge(Isrc, Idest, Convert.ToInt32(item.Distance));

            }
            return graph;
        }
    }

}

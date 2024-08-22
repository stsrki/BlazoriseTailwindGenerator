using System.Reflection;
using System.Xml;
using System.Text.Json;
using Blazorise;
using Blazorise.Tailwind.Providers;

namespace BlazoriseTailwindGenerator
{
    internal class Program
    {
        static void Main( string[] args )
        {
            ExtractTailwindClasses();
        }

        static string[] IgnoreClasses = [];

        static HashSet<string> distinctClasses = new HashSet<string>();
        static List<string> manualRAWList = [
            "w-8 h-4 after:h-3 after:w-3 after:top-1 after:left-[3px]",
            "w-9 h-5 after:h-4 after:w-4 after:top-[2px] after:left-[2px]",
             "w-12 h-7 after:h-5 after:w-5 after:top-1 after:left-[3px]",
            "w-14 h-8 after:h-6 after:w-6 after:top-1 after:left-[3px]",
            "w-16 h-10 after:h-7 after:w-7 after:top-1.5 after:left-[3px]",
            "w-11 h-6 after:h-5 after:w-5 after:top-0.5 after:left-[2px]",
            "ml-3 text-sm font-medium text-gray-900 dark:text-gray-300",
            "bg-gray-200 rounded-full peer peer-focus:ring-4 dark:bg-gray-700 peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:bg-white after:border-gray-300 after:border after:rounded-full after:transition-all dark:border-gray-600",
            "peer-focus:ring-primary-300 dark:peer-focus:ring-primary-800 peer-checked:bg-primary-600",
            "peer-focus:ring-secondary-300 dark:peer-focus:ring-secondary-800 peer-checked:bg-secondary-600",
            "peer-focus:ring-success-300 dark:peer-focus:ring-success-800 peer-checked:bg-success-600",
            "peer-focus:ring-danger-300 dark:peer-focus:ring-danger-800 peer-checked:bg-danger-600",
            "peer-focus:ring-warning-300 dark:peer-focus:ring-warning-800 peer-checked:bg-warning-600",
            "peer-focus:ring-info-300 dark:peer-focus:ring-info-800 peer-checked:bg-info-600",
            "peer-focus:ring-light-300 dark:peer-focus:ring-light-800 peer-checked:bg-light-600",
            "peer-focus:ring-dark-300 dark:peer-focus:ring-dark-800 peer-checked:bg-dark-600",
            "peer-focus:ring-link-300 dark:peer-focus:ring-link-800 peer-checked:bg-link-600",
            "relative focus:ring-4 font-medium focus:outline-none",
            "rounded-none first:rounded-l-lg last:rounded-r-lg",
            "rounded-none first:rounded-t-lg last:rounded-b-lg w-full",
            "text-white bg-secondary-700 hover:bg-secondary-800 focus:ring-secondary-300 dark:bg-secondary-600 dark:hover:bg-secondary-700 dark:focus:ring-secondary-800",
            "text-white bg-secondary-500 hover:bg-secondary-600 focus:ring-secondary-100 dark:bg-secondary-400 dark:hover:bg-secondary-500 dark:focus:ring-secondary-600",
            "cursor-not-allowed opacity-60",
            "text-xs font-medium text-center p-0.5 leading-none rounded-full bg-striped bg-striped-animated progress-bar-indeterminate",
            "relative w-full h-full modal-fullscreen h-screen w-screen max-w-none h-full m-0 modal-dialog-centered modal-dialog-scrollable",
            "b-dropdown-toggle-arrow hidden w-4 h-4 ml-1",
            "absolute inset-y-0 left-0 pl-2.5 flex items-center pointer-events-none text-secondary-400 flex items-center self-center pl-1 w-4 h-4 rounded shadow border",
            "items-center float-right text-gray-400 bg-transparent hover:text-gray-900 p-1.5 ml-auto dark:hover:text-white rounded-lg",
            "ms-auto -mx-1.5 -my-1.5 bg-white items-center justify-center flex-shrink-0 text-gray-400 hover:text-gray-900 rounded-lg focus:ring-2 focus:ring-gray-300 p-1.5 hover:bg-gray-100 inline-flex h-8 w-8 dark:text-gray-500 dark:hover:text-white dark:bg-gray-800 dark:hover:bg-gray-700",
            "items-center text-gray-400 bg-transparent hover:text-gray-900 rounded-lg p-0.5 ml-2 dark:hover:text-white rounded-lg",
            "bg-blue-100 text-blue-500 focus:ring-2 focus:ring-blue-400 hover:bg-blue-200 dark:bg-blue-200 dark:text-blue-600 dark:hover:bg-blue-300",
            "bg-secondary-100 text-secondary-500 focus:ring-2 focus:ring-secondary-400 hover:bg-secondary-200 dark:bg-secondary-200 dark:text-secondary-600 dark:hover:bg-secondary-300",
            "bg-green-100 text-green-500 focus:ring-2 focus:ring-green-400 hover:bg-green-200 dark:bg-green-200 dark:text-green-600 dark:hover:bg-green-300",
            "bg-red-100 text-red-500 focus:ring-2 focus:ring-red-400 hover:bg-red-200 dark:bg-red-200 dark:text-red-600 dark:hover:bg-red-300",
            "bg-yellow-100 text-yellow-500 focus:ring-2 focus:ring-yellow-400 hover:bg-yellow-200 dark:bg-yellow-200 dark:text-yellow-600 dark:hover:bg-yellow-300",
            "bg-info-100 text-info-500 focus:ring-2 focus:ring-info-400 hover:bg-info-200 dark:bg-info-200 dark:text-info-600 dark:hover:bg-info-300",
            "bg-light-100 text-light-500 focus:ring-2 focus:ring-light-400 hover:bg-light-200 dark:bg-light-200 dark:text-light-600 dark:hover:bg-light-300",
            "bg-dark-800 text-dark-100 focus:ring-2 focus:ring-dark-400 hover:bg-dark-700 dark:bg-dark-800 dark:text-dark-400 dark:hover:bg-dark-700",
            "text-primary-600 dark:text-primary-500 hover:underline",
            "text-gray-400 bg-transparent hover:text-gray-900 ml-auto dark:hover:text-white",
            "bg-blue-100 text-blue-500 focus:ring-2 focus:ring-blue-400 hover:bg-blue-200 dark:bg-blue-200 dark:text-blue-600 dark:hover:bg-blue-300",
            "bg-secondary-100 text-secondary-500 focus:ring-2 focus:ring-secondary-400 hover:bg-secondary-200 dark:bg-secondary-200 dark:text-secondary-600 dark:hover:bg-secondary-300",
            "bg-green-100 text-green-500 focus:ring-2 focus:ring-green-400 hover:bg-green-200 dark:bg-green-200 dark:text-green-600 dark:hover:bg-green-300",
            "bg-red-100 text-red-500 focus:ring-2 focus:ring-red-400 hover:bg-red-200 dark:bg-red-200 dark:text-red-600 dark:hover:bg-red-300",
            "bg-yellow-100 text-yellow-500 focus:ring-2 focus:ring-yellow-400 hover:bg-yellow-200 dark:bg-yellow-200 dark:text-yellow-600 dark:hover:bg-yellow-300",
            "bg-info-100 text-info-500 focus:ring-2 focus:ring-info-400 hover:bg-info-200 dark:bg-info-200 dark:text-info-600 dark:hover:bg-info-300",
            "bg-light-100 text-light-500 focus:ring-2 focus:ring-light-400 hover:bg-light-200 dark:bg-light-200 dark:text-light-600 dark:hover:bg-light-300",
            "bg-dark-800 text-dark-100 focus:ring-2 focus:ring-dark-400 hover:bg-dark-700 dark:bg-dark-800 dark:text-dark-400 dark:hover:bg-dark-700",
            "text-primary-600 dark:text-primary-500 hover:underline",
            "text-gray-400 bg-transparent hover:text-gray-900 ml-auto dark:hover:text-white",
            "absolute top-0 left-0 z-30 flex items-center justify-center h-full px-4 cursor-pointer group focus:outline-none",
            "inline-flex items-center justify-center w-8 h-8 rounded-full sm:w-10 sm:h-10 bg-white/30 dark:bg-gray-800/30 group-hover:bg-white/50 dark:group-hover:bg-gray-800/60 group-focus:ring-4 group-focus:ring-white dark:group-focus:ring-gray-800/70 group-focus:outline-none",
            "w-5 h-5 text-white sm:w-6 sm:h-6 dark:text-gray-800" ,
            "absolute top-0 right-0 z-30 flex items-center justify-center h-full px-4 cursor-pointer group focus:outline-none",
            "inline-flex items-center justify-center w-8 h-8 rounded-full sm:w-10 sm:h-10 bg-white/30 dark:bg-gray-800/30 group-hover:bg-white/50 dark:group-hover:bg-gray-800/60 group-focus:ring-4 group-focus:ring-white dark:group-focus:ring-gray-800/70 group-focus:outline-none",
            "rounded-none first:rounded-l-lg last:rounded-r-lg rounded-lg",
            "ml-1 text-sm font-medium text-gray-500 md:ml-2 dark:text-gray-400",
            "inline-flex items-center space-x-1 md:space-x-3",
            "flex flex-col p-4 mt-4 md:flex-row md:mt-0 md:text-sm md:font-medium",
            "list-none",
            "flex flex-col p-4 mt-4 md:flex-row md:mt-0 md:text-sm md:font-medium",
            "b-bar-dropdown-toggle-arrow w-4 h-4 ml-1",
            "py-1 text-sm text-gray-700 dark:text-gray-400",
            "flex flex-wrap items-center justify-between mx-auto",
            "w-3.5 h-3.5",
            "bg-blue-100 text-blue-500 focus:ring-2 focus:ring-blue-400 hover:bg-blue-200 dark:bg-blue-200 dark:text-blue-600 dark:hover:bg-blue-300",
            "bg-secondary-100 text-secondary-500 focus:ring-2 focus:ring-secondary-400 hover:bg-secondary-200 dark:bg-secondary-200 dark:text-secondary-600 dark:hover:bg-secondary-300",
            "bg-green-100 text-green-500 focus:ring-2 focus:ring-green-400 hover:bg-green-200 dark:bg-green-200 dark:text-green-600 dark:hover:bg-green-300",
            "bg-red-100 text-red-500 focus:ring-2 focus:ring-red-400 hover:bg-red-200 dark:bg-red-200 dark:text-red-600 dark:hover:bg-red-300",
            "bg-yellow-100 text-yellow-500 focus:ring-2 focus:ring-yellow-400 hover:bg-yellow-200 dark:bg-yellow-200 dark:text-yellow-600 dark:hover:bg-yellow-300",
            "bg-info-100 text-info-500 focus:ring-2 focus:ring-info-400 hover:bg-info-200 dark:bg-info-200 dark:text-info-600 dark:hover:bg-info-300",
            "bg-light-100 text-light-500 focus:ring-2 focus:ring-light-400 hover:bg-light-200 dark:bg-light-200 dark:text-light-600 dark:hover:bg-light-300",
            "bg-dark-800 text-dark-100 focus:ring-2 focus:ring-dark-400 hover:bg-dark-700 dark:bg-dark-800 dark:text-dark-400 dark:hover:bg-dark-700",
            "text-primary-600 dark:text-primary-500 hover:underline",
            "text-gray-400 bg-transparent hover:text-gray-900 ml-auto dark:hover:text-white",
            "w-6 h-6 shrink-0 rotate-180",
            "w-6 h-6 shrink-0",
            "block px-4 py-2 cursor-pointer",
            "font-medium hover:bg-gray-100 dark:hover:bg-gray-600",
            "bg-primary-700",
            "rounded",
            "text-white bg-primary-700 md:bg-transparent md:text-primary-700 md:dark:text-white dark:bg-primary-600 md:dark:bg-transparent",
            "text-gray-700 hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-primary-700 dark:text-gray-400 dark:hover:bg-gray-700 md:dark:hover:bg-transparent",
            "fill-primary-200",
            "fill-secondary-200",
            "fill-success-200",
            "fill-danger-200",
            "fill-warning-200",
            "fill-info-200",
            "fill-light-700",
            "fill-dark-200",
            "fill-primary-600",
            "fill-gray-200",
            "inline mr-3 w-4 h-4 text-white animate-spin",
            "b-toast p-4 text-gray-500 bg-white rounded-lg shadow dark:text-gray-400 dark:bg-gray-800 [&:not(:last-child)]:mb-4",
        ];

        static void ExtractTailwindClasses()
        {
            var provider = new TailwindClassProvider();
            Type providerType = provider.GetType();

            foreach ( MethodInfo method in providerType.GetMethods( BindingFlags.Public | BindingFlags.Instance ) )
            {
                if ( method.ReturnType == typeof( string ) &&
                    ( !method.Name.StartsWith( "To" ) || method.Name.StartsWith( "Tooltip" ) || method.Name.StartsWith( "Toast" ) ) )
                {
                    var classCombinations = InvokeMethodWithAllEnumCombinations( provider, method );

                    foreach ( var classCombination in classCombinations )
                    {
                        distinctClasses.Add( classCombination );
                    }
                }
            }

            var manualList = ( from mrl in manualRAWList
                             .SelectMany( x => x.Split( ' ' ) )
                               select mrl ).ToList();

            distinctClasses.UnionWith( manualList );

            var safelist = ( from dc in distinctClasses
                             let className = dc
                             where !className.StartsWith( ':' ) && !className.EndsWith( ':' )
                             && !className.StartsWith( "!-" )
                             && !className.EndsWith( '-' )
                             select className ).ToList();

            safelist.AddRange( ( from c in safelist
                                 where c.StartsWith( '!' )
                                 let nc = c.Trim( '!' )
                                 select c ).ToList() );

            //var safelist = distinctClasses
            //    .Where( x => !x.StartsWith( ':' ) && !x.EndsWith( ':' ) )
            //    //.Where( x => x.StartsWith( "b-" ) || allClasses.Contains( x ) )
            //    .OrderBy( x => x ).ToList();

            string jsonSafelist = JsonSerializer.Serialize( safelist.Order(), new JsonSerializerOptions { WriteIndented = true } );

            string moduleContent = $"module.exports = {jsonSafelist};";

            File.WriteAllText( @"D:\Projects\Megabit\Blazorise\Source\Blazorise.Tailwind\wwwroot\tailwind.safelist.config.js", moduleContent );
        }

        private static List<List<object>> GenerateParameterSets( ParameterInfo[] parameterInfos )
        {
            var parameterSets = new List<List<object>>();
            foreach ( var param in parameterInfos )
            {
                var paramType = param.ParameterType;
                var values = GetValuesForParameter( paramType );
                parameterSets.Add( values );
            }
            return parameterSets;
        }

        private static List<object> GetValuesForParameter( Type paramType )
        {
            var values = new List<object>();

            if ( paramType.IsEnum )
            {
                foreach ( var value in Enum.GetValues( paramType ) )
                {
                    values.Add( value );
                }
            }
            else if ( paramType == typeof( bool ) )
            {
                values.Add( true );
                values.Add( false );
            }
            else if ( paramType == typeof( bool? ) )
            {
                values.Add( null );
                values.Add( true );
                values.Add( false );
            }
            else if ( paramType.IsClass && paramType == typeof( Color ) )
            {
                values.Add( Color.Primary );
                values.Add( Color.Secondary );
                values.Add( Color.Success );
                values.Add( Color.Danger );
                values.Add( Color.Warning );
                values.Add( Color.Info );
                values.Add( Color.Light );
                values.Add( Color.Dark );
                values.Add( Color.Link );
            }
            else if ( paramType.IsClass && paramType == typeof( Background ) )
            {
                values.Add( Background.Primary );
                values.Add( Background.Secondary );
                values.Add( Background.Success );
                values.Add( Background.Danger );
                values.Add( Background.Warning );
                values.Add( Background.Info );
                values.Add( Background.Light );
                values.Add( Background.Dark );
                values.Add( Background.White );
                values.Add( Background.Transparent );
                values.Add( Background.Body );
            }
            else if ( paramType.IsClass && paramType == typeof( TextColor ) )
            {
                values.Add( TextColor.Primary );
                values.Add( TextColor.Secondary );
                values.Add( TextColor.Success );
                values.Add( TextColor.Danger );
                values.Add( TextColor.Warning );
                values.Add( TextColor.Info );
                values.Add( TextColor.Light );
                values.Add( TextColor.Dark );
                values.Add( TextColor.Body );
                values.Add( TextColor.Muted );
                values.Add( TextColor.White );
                values.Add( TextColor.Black50 );
                values.Add( TextColor.White50 );
            }
            else if ( paramType == typeof( RowColumnsDefinition ) )
            {
                var breakpoints = Enum.GetValues( typeof( Breakpoint ) ).Cast<Breakpoint>();
                foreach ( var bp in breakpoints )
                    values.Add( new RowColumnsDefinition { Breakpoint = bp } );
            }
            else if ( paramType == typeof( GridRowsDefinition ) )
            {
                var breakpoints = Enum.GetValues( typeof( Breakpoint ) ).Cast<Breakpoint>();
                foreach ( var bp in breakpoints )
                    values.Add( new GridRowsDefinition { Breakpoint = bp } );
            }
            else if ( paramType == typeof( GridColumnsDefinition ) )
            {
                var breakpoints = Enum.GetValues( typeof( Breakpoint ) ).Cast<Breakpoint>();
                foreach ( var bp in breakpoints )
                    values.Add( new GridColumnsDefinition { Breakpoint = bp } );
            }
            else if ( paramType == typeof( IEnumerable<ColumnDefinition> ) )
            {
                var columnDefinitions = new List<ColumnDefinition>();

                var columnWidths = Enum.GetValues( typeof( ColumnWidth ) ).Cast<ColumnWidth>();
                var breakpoints = Enum.GetValues( typeof( Breakpoint ) ).Cast<Breakpoint>();
                var conditions = new bool[] { true, false };

                foreach ( var cw in columnWidths )
                    foreach ( var bp in breakpoints )
                        foreach ( var cond in conditions )
                            columnDefinitions.Add( new ColumnDefinition { ColumnWidth = cw, Breakpoint = bp, Offset = cond } );

                values.Add( columnDefinitions );
            }
            else if ( paramType == typeof( DisplayDefinition ) )
            {
                var breakpoints = Enum.GetValues( typeof( Breakpoint ) ).Cast<Breakpoint>();
                var directions = Enum.GetValues( typeof( DisplayDirection ) ).Cast<DisplayDirection>();
                var conditions = new bool?[] { null, true, false };

                foreach ( var bp in breakpoints )
                    foreach ( var dir in directions )
                        foreach ( var cond in conditions )
                            values.Add( new DisplayDefinition { Breakpoint = bp, Direction = dir, Condition = cond } );
            }
            else if ( paramType == typeof( TextSizeDefinition ) )
            {
                var breakpoints = Enum.GetValues( typeof( Breakpoint ) ).Cast<Breakpoint>();
                foreach ( var bp in breakpoints )
                    values.Add( new TextSizeDefinition { Breakpoint = bp } );
            }
            else if ( paramType == typeof( IEnumerable<FlexDefinition> ) )
            {
                var flexDefinitions = new List<FlexDefinition>();

                var flexTypes = Enum.GetValues( typeof( FlexType ) ).Cast<FlexType>();
                var breakpoints = Enum.GetValues( typeof( Breakpoint ) ).Cast<Breakpoint>();
                var flexDirections = Enum.GetValues( typeof( FlexDirection ) ).Cast<FlexDirection>();
                var flexJustifyContents = Enum.GetValues( typeof( FlexJustifyContent ) ).Cast<FlexJustifyContent>();
                var flexAlignItems = Enum.GetValues( typeof( FlexAlignItems ) ).Cast<FlexAlignItems>();
                var flexAlignSelves = Enum.GetValues( typeof( FlexAlignSelf ) ).Cast<FlexAlignSelf>();
                var flexAlignContents = Enum.GetValues( typeof( FlexAlignContent ) ).Cast<FlexAlignContent>();
                var flexGrowShrinks = Enum.GetValues( typeof( FlexGrowShrink ) ).Cast<FlexGrowShrink>();
                var flexGrowShrinkSizes = Enum.GetValues( typeof( FlexGrowShrinkSize ) ).Cast<FlexGrowShrinkSize>();
                var flexWraps = Enum.GetValues( typeof( FlexWrap ) ).Cast<FlexWrap>();
                var flexOrders = Enum.GetValues( typeof( FlexOrder ) ).Cast<FlexOrder>();
                var fills = new bool[] { true, false };
                var conditions = new bool?[] { null, true, false };

                foreach ( var bp in breakpoints )
                    foreach ( var fd in flexDirections )
                        flexDefinitions.Add( new FlexDefinition
                        {
                            Breakpoint = bp,
                            Direction = fd,
                        } );

                foreach ( var bp in breakpoints )
                    foreach ( var fjc in flexJustifyContents )
                        flexDefinitions.Add( new FlexDefinition
                        {
                            Breakpoint = bp,
                            JustifyContent = fjc,
                        } );

                foreach ( var bp in breakpoints )
                    foreach ( var fai in flexAlignItems )
                        flexDefinitions.Add( new FlexDefinition
                        {
                            Breakpoint = bp,
                            AlignItems = fai,
                        } );

                foreach ( var bp in breakpoints )
                    foreach ( var fas in flexAlignSelves )
                        flexDefinitions.Add( new FlexDefinition
                        {
                            Breakpoint = bp,
                            AlignSelf = fas,
                        } );

                foreach ( var bp in breakpoints )
                    foreach ( var fac in flexAlignContents )
                        flexDefinitions.Add( new FlexDefinition
                        {
                            Breakpoint = bp,
                            AlignContent = fac,
                        } );

                foreach ( var bp in breakpoints )
                    foreach ( var fgs in flexGrowShrinks )
                        foreach ( var fgsz in flexGrowShrinkSizes )
                            flexDefinitions.Add( new FlexDefinition
                            {
                                Breakpoint = bp,
                                GrowShrink = fgs,
                                GrowShrinkSize = fgsz,
                            } );

                foreach ( var bp in breakpoints )
                    foreach ( var fw in flexWraps )
                        flexDefinitions.Add( new FlexDefinition
                        {
                            Breakpoint = bp,
                            Wrap = fw,
                        } );

                foreach ( var bp in breakpoints )
                    foreach ( var fo in flexOrders )
                        flexDefinitions.Add( new FlexDefinition
                        {
                            Breakpoint = bp,
                            Order = fo,
                        } );

                foreach ( var bp in breakpoints )
                    foreach ( var fill in fills )
                        flexDefinitions.Add( new FlexDefinition
                        {
                            Breakpoint = bp,
                            Fill = fill,
                        } );

                foreach ( var bp in breakpoints )
                    foreach ( var cond in conditions )
                        flexDefinitions.Add( new FlexDefinition
                        {
                            Breakpoint = bp,
                            Condition = cond,
                        } );

                values.Add( flexDefinitions );
            }
            else if ( paramType == typeof( IEnumerable<SizingDefinition> ) )
            {
                var sizingDefinitions = new List<SizingDefinition>();

                var breakpoints = Enum.GetValues( typeof( Breakpoint ) ).Cast<Breakpoint>();
                var isMins = new bool[] { true, false };
                var isMaxs = new bool[] { true, false };
                var isViewports = new bool[] { true, false };

                foreach ( var bp in breakpoints )
                    foreach ( var isMin in isMins )
                        sizingDefinitions.Add( new SizingDefinition { Breakpoint = bp, IsMin = isMin } );

                foreach ( var bp in breakpoints )
                    foreach ( var isMax in isMaxs )
                        sizingDefinitions.Add( new SizingDefinition { Breakpoint = bp, IsMax = isMax } );

                foreach ( var bp in breakpoints )
                    foreach ( var isViewport in isViewports )
                        sizingDefinitions.Add( new SizingDefinition { Breakpoint = bp, IsViewport = isViewport } );

                values.Add( sizingDefinitions );
            }
            else if ( paramType == typeof( IEnumerable<(PositionEdgeType edgeType, int edgeOffset)> ) )
            {
                var edges = new List<(PositionEdgeType edgeType, int edgeOffset)>();

                var positionEdgeTypes = Enum.GetValues( typeof( PositionEdgeType ) ).Cast<PositionEdgeType>();
                var edgeOffsets = new int[] { 0, 50, 100 };

                foreach ( var edgeType in positionEdgeTypes )
                    foreach ( var edgeOffset in edgeOffsets )
                        edges.Add( (edgeType, edgeOffset) );

                values.Add( edges );
            }
            else if ( paramType == typeof( IEnumerable<(BorderSide borderSide, BorderColor borderColor)> ) )
            {
                var borderRules = new List<(BorderSide borderSide, BorderColor borderColor)>();

                var borderSides = Enum.GetValues( typeof( BorderSide ) ).Cast<BorderSide>();

                foreach ( var borderSide in borderSides )
                {
                    borderRules.Add( (borderSide, BorderColor.Primary) );
                    borderRules.Add( (borderSide, BorderColor.Secondary) );
                    borderRules.Add( (borderSide, BorderColor.Success) );
                    borderRules.Add( (borderSide, BorderColor.Danger) );
                    borderRules.Add( (borderSide, BorderColor.Warning) );
                    borderRules.Add( (borderSide, BorderColor.Info) );
                    borderRules.Add( (borderSide, BorderColor.Light) );
                    borderRules.Add( (borderSide, BorderColor.Dark) );
                    borderRules.Add( (borderSide, BorderColor.White) );
                }

                values.Add( borderRules );
            }
            else if ( paramType == typeof( IEnumerable<(Side side, Breakpoint breakpoint)> ) )
            {
                var sizingRules = new List<(Side side, Breakpoint breakpoint)>();

                var breakpoints = Enum.GetValues( typeof( Breakpoint ) ).Cast<Breakpoint>();
                var sides = Enum.GetValues( typeof( Side ) ).Cast<Side>();

                foreach ( var side in sides )
                    foreach ( var bp in breakpoints )
                        sizingRules.Add( (side, bp) );

                values.Add( sizingRules );
            }
            else if ( paramType == typeof( IEnumerable<GapSide> ) )
            {
                var gapSideRules = new List<GapSide>();

                var gapSides = Enum.GetValues( typeof( GapSide ) ).Cast<GapSide>();

                foreach ( var gapSide in gapSides )
                    gapSideRules.Add( gapSide );

                values.Add( gapSideRules );
            }
            else
            {
                values.Add( paramType.IsValueType ? Activator.CreateInstance( paramType ) : null );
            }

            return values;
        }

        private static IEnumerable<string> InvokeMethodWithAllEnumCombinations( TailwindClassProvider provider, MethodInfo method )
        {
            var parameterInfos = method.GetParameters();
            var parameterSets = GenerateParameterSets( parameterInfos );

            foreach ( var combination in GetCartesianProduct( parameterSets ) )
            {
                string result = null;
                try
                {
                    result = method.Invoke( provider, combination.ToArray() ) as string;
                }
                catch ( Exception ex )
                {
                    Console.WriteLine( $"Error invoking '{method.Name}' with params ({string.Join( ", ", combination )}): {ex.Message}" );
                }

                if ( !string.IsNullOrEmpty( result ) )
                {
                    foreach ( var className in result.Split( ' ', StringSplitOptions.RemoveEmptyEntries ) )
                    {
                        yield return className;
                    }
                }
            }
        }

        private static IEnumerable<IEnumerable<object>> GetCartesianProduct( List<List<object>> lists )
        {
            IEnumerable<IEnumerable<object>> result = new[] { Enumerable.Empty<object>() };
            foreach ( var list in lists )
            {
                result = from seq in result
                         from item in list
                         select seq.Concat( new[] { item } );
            }
            return result;
        }
    }
}

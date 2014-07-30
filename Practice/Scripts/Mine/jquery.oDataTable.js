if (!jQuery) { throw new Error("ODataTable requires jQuery") }

$.fn.oDataTable = function (props) {
		// local variables
		var target = this;
        var selectorDDL = $('<select>').addClass('.selector').appendTo(target);
        var table = $('<table>').appendTo($('<div>').addClass('results').appendTo(target));

		// functions
        var tableInitilized = false;
        function initTable() {
            table.find('tr th').off('click');
            table.html('');
            
            var cols = props.table.columns;
            if (cols && cols.length) {
            	var names = props.table.columnNames;
            	if (names)
            		createTableHeading(table, cols, names);
            	else
            		createTableHeading(table, cols, cols);
            	tableInitilized = true;
            }
            else {
            	tableInitilized = false;
            }
            
        }

        function createTableHeading(table, cols, names) {
        	var headLine = $('<tr>');
        	var row, ind;
        	for (var i = 0, item, name; item = cols[i]; i++) {
        		name = names[i] ? names[i] : item;
        		row = $('<th>').attr('data-colname', item).html(name).click(thClickSort);
        		ind = sort.indexOf(item);
        		if (ind != -1)
        			if (desc[ind]) row.addClass('asc');
        			else row.addClass('desc');
        		headLine.append(row);
        	}
        	table.append(headLine);
        }

        function thClickSort() {
            var name = $(this).attr('data-colname');
            var ind = sort.indexOf(name);
            if (ind === -1) {
                sort.push(name);
                desc.push(false);
            }
            else {
                sort.splice(ind, 1);
                var d = desc[ind];
                desc.splice(ind, 1);
                if (!d) {
                    sort.push(name);
                    desc.push(true);
                }
            }
            tableRefresh();
        }

        function buildFilterClause() {
            return '$filter=' + props.selector.filterFieldName + ' eq ' + selectorDDL.val();
        }

        var sort = [];
        var desc = [];
        function buildSortClause() {
            if (!(sort && sort.length)) return '';
            var res = '$orderby='
            for (var i = 0, item; item = sort[i]; i++) {
                res += item;
                desc[i] && (res += ' desc');
                res += ',';
            }
            return res.substring(0, res.length - 1);
        }

        var tableRefresh = function () {
            var url = props.table.dataSource + (props.table.dataSource.indexOf('?') > -1 ? '&' : '?') + buildFilterClause() + '&' + buildSortClause();
            console.log(url);
            $.get(url, null, function (data) {
            	if (data && data.value && data.value.length) {
            		initTable();
            		if (!tableInitilized) {
            			var arr = [];
            			for (var i in data.value[0]) {
            				arr.push(i);
            			}
            			createTableHeading(table, arr, arr);
            		}
            		var cols = tableInitilized ? props.table.columns : arr;
            		var reads = props.table.customReadScenarios;
            		for (var i = 0, item; item = data.value[i]; i++) {
            			var line = $('<tr>').appendTo(table);
            			for (var j = 0, col; col = cols[j]; j++) {
            				$('<td>').html(reads && reads[col] ? reads[col](item) : item[col]).appendTo(line);
            			}
            		}
            	}
            });
        };

		// init code itself
        $.get(props.selector.url, null, function (data) {
            selectorDDL.html('');
            var vf = props.selector.valueField, tf = props.selector.textField;
            for (var i = 0, item; item = data.value[i]; i++) {
                selectorDDL.append($("<option>").attr('value', item[vf]).html(item[tf]))
            }
            target.change(function () {
                tableRefresh();
            });
            tableRefresh();
        });

    };
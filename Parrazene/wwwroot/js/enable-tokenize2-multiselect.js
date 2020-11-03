
//Initilaise
var substringMatcher = function (strs) {
    return function findMatches(q, cb) {
        var matches, substringRegex;

        // an array that will be populated with substring matches
        matches = [];

        // regex used to determine if a string contains the substring `q`
        substrRegex = new RegExp(q, 'i');

        // iterate through the pool of strings and for any string that
        // contains the substring `q`, add it to the `matches` array
        $.each(strs, function (i, str) {
            if (substrRegex.test(str)) {
                matches.push(str);
            }
        });

        cb(matches);
    };
}


$('.auto-multi-select').tokenize2({
    placeholder:"Select user",
    dataSource: function (query, object) {
        $.get('/Chat/GetUsers', { query: query }, function (data) {
            var $items = [];
            $.each(data, function (k,v) {
                console.log(v);
                if ((v.name.toLowerCase()).search(query.toLowerCase()) > -1) {
                    $items.push({value: v.id, text: v.name});
                }
            });
            object.trigger('tokenize:dropdown:fill', [$items, true]);
        });
    }
}
);


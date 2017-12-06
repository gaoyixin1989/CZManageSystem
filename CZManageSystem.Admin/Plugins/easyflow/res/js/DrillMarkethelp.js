var Drill = { data: null, bind: function (a) { this.data = a; }, getArray: function (a,b) {
   
    var arrary = new Array();
    if (!b) {
        for (var i = 0; i < this.data.length; i++) {

            arrary.push(this.data[i][a]);
        }
    } else {
        for (var i = 0; i < this.data.length; i++) {

            arrary.push(this.data[i][a] / this.data[i][b]);
        }
    }
    return arrary;
}
}
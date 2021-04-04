Handlebars.registerPartial("fiche", Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    return "<div class=\"fiche-"
    + container.escapeExpression(container.lambda(depth0, depth0))
    + "\"></div>";
},"useData":true}));
this["spa_templates"] = this["spa_templates"] || {};
this["spa_templates"]["templates"] = this["spa_templates"]["templates"] || {};
this["spa_templates"]["templates"]["api"] = Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    return "<img src=\""
    + container.escapeExpression(container.lambda(depth0, depth0))
    + "\"></img>";
},"useData":true});
this["spa_templates"]["templates"]["body"] = Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var helper, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<section class=\"body\">\r\n    "
    + container.escapeExpression(((helper = (helper = lookupProperty(helpers,"bericht") || (depth0 != null ? lookupProperty(depth0,"bericht") : depth0)) != null ? helper : container.hooks.helperMissing),(typeof helper === "function" ? helper.call(depth0 != null ? depth0 : (container.nullContext || {}),{"name":"bericht","hash":{},"data":data,"loc":{"start":{"line":2,"column":4},"end":{"line":2,"column":15}}}) : helper)))
    + "\r\n</section>";
},"useData":true});
this["spa_templates"]["templates"]["bord"] = Handlebars.template({"1":function(container,depth0,helpers,partials,data) {
    var stack1, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return ((stack1 = lookupProperty(helpers,"each").call(depth0 != null ? depth0 : (container.nullContext || {}),depth0,{"name":"each","hash":{},"fn":container.program(2, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":3,"column":8},"end":{"line":7,"column":17}}})) != null ? stack1 : "");
},"2":function(container,depth0,helpers,partials,data) {
    var stack1, helper, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "        <div id=\""
    + container.escapeExpression(((helper = (helper = lookupProperty(helpers,"index") || (data && lookupProperty(data,"index"))) != null ? helper : container.hooks.helperMissing),(typeof helper === "function" ? helper.call(depth0 != null ? depth0 : (container.nullContext || {}),{"name":"index","hash":{},"data":data,"loc":{"start":{"line":4,"column":17},"end":{"line":4,"column":27}}}) : helper)))
    + "\"class=\"bordTegel\" onclick=\"Game.Reversi.showFiche(this)\">\r\n"
    + ((stack1 = container.invokePartial(lookupProperty(partials,"fiche"),depth0,{"name":"fiche","data":data,"indent":"            ","helpers":helpers,"partials":partials,"decorators":container.decorators})) != null ? stack1 : "")
    + "        </div>\r\n";
},"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var stack1, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<div id=\"bord\">\r\n"
    + ((stack1 = lookupProperty(helpers,"each").call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? lookupProperty(depth0,"bord") : depth0),{"name":"each","hash":{},"fn":container.program(1, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":2,"column":4},"end":{"line":8,"column":13}}})) != null ? stack1 : "")
    + "</div>\r\n";
},"usePartial":true,"useData":true});
this["spa_templates"]["templates"]["huidigeSpeler"] = Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var helper, alias1=depth0 != null ? depth0 : (container.nullContext || {}), alias2=container.hooks.helperMissing, alias3="function", alias4=container.escapeExpression, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<label class=\"text-speler\">Omschrijving: "
    + alias4(((helper = (helper = lookupProperty(helpers,"omschrijving") || (depth0 != null ? lookupProperty(depth0,"omschrijving") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"omschrijving","hash":{},"data":data,"loc":{"start":{"line":1,"column":41},"end":{"line":1,"column":57}}}) : helper)))
    + "</label>\r\n<label class=\"text-speler\">Jij bent: "
    + alias4(((helper = (helper = lookupProperty(helpers,"kleur") || (depth0 != null ? lookupProperty(depth0,"kleur") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"kleur","hash":{},"data":data,"loc":{"start":{"line":2,"column":37},"end":{"line":2,"column":46}}}) : helper)))
    + "</label>";
},"useData":true});
this["spa_templates"]["templates"]["spelers"] = Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var helper, alias1=depth0 != null ? depth0 : (container.nullContext || {}), alias2=container.hooks.helperMissing, alias3="function", alias4=container.escapeExpression, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<label class=\"text-"
    + alias4(((helper = (helper = lookupProperty(helpers,"aanzet") || (depth0 != null ? lookupProperty(depth0,"aanzet") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"aanzet","hash":{},"data":data,"loc":{"start":{"line":1,"column":19},"end":{"line":1,"column":29}}}) : helper)))
    + "\">"
    + alias4(((helper = (helper = lookupProperty(helpers,"speler") || (depth0 != null ? lookupProperty(depth0,"speler") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"speler","hash":{},"data":data,"loc":{"start":{"line":1,"column":31},"end":{"line":1,"column":41}}}) : helper)))
    + ": "
    + alias4(((helper = (helper = lookupProperty(helpers,"kleur") || (depth0 != null ? lookupProperty(depth0,"kleur") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"kleur","hash":{},"data":data,"loc":{"start":{"line":1,"column":43},"end":{"line":1,"column":52}}}) : helper)))
    + "</label>\r\n";
},"useData":true});
this["spa_templates"]["templates"]["stats"] = Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var helper, alias1=depth0 != null ? depth0 : (container.nullContext || {}), alias2=container.hooks.helperMissing, alias3="function", alias4=container.escapeExpression, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<canvas id=\"PlayerPointsChart\" width=\"1600\" height=\"900\"></canvas>\r\n\r\n<script>\r\n    var ctx = document.getElementById('PlayerPointsChart').getContext('2d');\r\n    var player1PieceHistory = ["
    + alias4(((helper = (helper = lookupProperty(helpers,"player1") || (depth0 != null ? lookupProperty(depth0,"player1") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"player1","hash":{},"data":data,"loc":{"start":{"line":5,"column":31},"end":{"line":5,"column":44}}}) : helper)))
    + "];\r\n    var player2PieceHistory = ["
    + alias4(((helper = (helper = lookupProperty(helpers,"player2") || (depth0 != null ? lookupProperty(depth0,"player2") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"player2","hash":{},"data":data,"loc":{"start":{"line":6,"column":31},"end":{"line":6,"column":44}}}) : helper)))
    + "];\r\n    var turns = [];\r\n    for (let i = 0; i < player1PieceHistory.length; i++) {\r\n        turns.push(\"Beurt \" + (i + 1));\r\n    }\r\n    var data = {\r\n        labels: turns,\r\n        datasets: [{\r\n            label: \"Wit\",\r\n            lineTension: 0.1,\r\n            borderColor: \"red\",\r\n            borderDash: [],\r\n            pointBorderColor: \"black\",\r\n            pointBackgroundColor: \"red\",\r\n            pointRadius: 4,\r\n            data: player1PieceHistory,\r\n            animation: \"none\",\r\n        }, {\r\n            label: \"Zwart\",\r\n            background: false,\r\n            lineTension: 0.1,\r\n            borderColor: \"blue\",\r\n            borderDash: [],\r\n            pointBorderColor: \"black\",\r\n            pointBackgroundColor: \"blue\",\r\n            pointRadius: 4,\r\n            data: player2PieceHistory,\r\n            animation: \"none\",\r\n        }\r\n        ]\r\n    };\r\n    var options = {\r\n        scales: {\r\n            yAxes: [{\r\n                ticks: {\r\n                    beginAtZero: true\r\n                },\r\n                scaleLabel: {\r\n                    display: true,\r\n                    labelString: 'Piece history',\r\n                    fontSize: 16\r\n                }\r\n            }]\r\n        }\r\n    };\r\n    new Chart(ctx, {\r\n        type: 'line',\r\n        data: data,\r\n        options: options\r\n    });\r\n</script>";
},"useData":true});
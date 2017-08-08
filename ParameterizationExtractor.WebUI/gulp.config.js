'use strict';

module.exports = {
    rootFolder: "./wwwroot/",
    rootJsFolder: "./wwwroot/js/",
    rootCssFolder: "./wwwroot/css/",
    rootAppFolder: "./wwwroot/app/",
    sources: {
        jsFilesInclSourcePaths: [
            "node_modules/core-js/client/shim.min.js",
            "node_modules/zone.js/dist/zone.js",
            "node_modules/reflect-metadata/Reflect.js",
            "node_modules/systemjs/dist/system.src.js",
            "node_modules/jquery/dist/jquery.js",
            "node_modules/bootstrap/dist/js/bootstrap.js",
            "node_modules/@ng-bootstrap/ng-bootstrap/bundles/ng-bootstrap.js",
            "node_modules/ng2-charts/bundles/ng2-charts.umd.js",
            "node_modules/file-saver/FileSaver.js"
        ],
        cssFiles: [
            "node_modules/bootstrap/dist/css/bootstrap.css",
            "node_modules/angular2-toaster/lib/toaster.css",
            "node_modules/ng2-slim-loading-bar/style.css",
            "./css/custom.css",
            "node_modules/angular2-busy/build/style/busy.css"
        ],
        angular: "node_modules/@angular/**/*.*",
        angularToastr: "node_modules/angular2-toaster/**/*.js",
        angularLoadingBar: "node_modules/ng2-loading-bar/**/*.js",
        Rxjs: "node_modules/rxjs/**/*.*",
        angularBusy: "node_modules/angular2-busy/**/*.*",
        angular2DynamicComponent: "node_modules/angular2-dynamic-component/**/*.*",
        tsMetadata: "node_modules/ts-metadata-helper/**/*.*",
        angular2Logger: "node_modules/angular2-logger/**/*.*",
        coreJs: "node_modules/core-js/**/*.*"
    }
};
var gulp = require('gulp');
var runSeq = require('run-sequence');
var del = require('del');

var buildConfig = require('../gulp.config');

gulp.task('build:dev', function (done) {
    runSeq(
        'clean-vendor-js-in-root',
        'clean-vendor-css-in-root',
        'clean-app-in-root',
        'copy-system-js',
        'copy-index-html',
        'copy-app',
        'copy-vendor-js-to-wwwroot',
        'copy-vendor-css-to-wwwroot',
        done);
});

gulp.task('clean-vendor-js-in-root', function (done) {
    del(buildConfig.rootJsFolder, { force: true }).then(function () {
        done();
    });
});

gulp.task('clean-vendor-css-in-root', function (done) {
    del(buildConfig.rootCssFolder, { force: true }).then(function () {
        done();
    });
});

gulp.task('clean-app-in-root', function (done) {
    del(buildConfig.rootAppFolder, { force: true }).then(function () {
        done();
    });
});

gulp.task('clean-root', function (done) {
    del(buildConfig.rootFolder+'*.*', { force: true }).then(function () {
        done();
    });
});

gulp.task('copy-vendor-js-to-wwwroot', function (done) {
    runSeq(
        'copy-angular',
        'copy-rxjs',
        'copy-toastr',
        'copy-loadingBar',
        'copy-angular2-busy',
        'copy-angular2-dynamic-component',
        'copy-ts-metadata-helper',
        'copy-angular2-logger',
        'copy-core-js',
        'copy-allOther',
        done);
});

gulp.task('copy-angular', function () {
    return gulp.src(buildConfig.sources.angular)
        .pipe(gulp.dest(buildConfig.rootJsFolder + '@angular/'));
});

gulp.task('copy-app', function () {
    return gulp.src([
        './app/**/*.js',
        './app/**/*.ts',
        './app/**/*.js.map',
        './app/**/*.html',
        './app/**/*.css',
    ])
        .pipe(gulp.dest(buildConfig.rootAppFolder));
});

gulp.task('copy-rxjs', function () {
    return gulp.src(buildConfig.sources.Rxjs)
        .pipe(gulp.dest(buildConfig.rootJsFolder + 'rxjs/'));
});

gulp.task('copy-angular2-busy', function () {
    return gulp.src(buildConfig.sources.angularBusy)
        .pipe(gulp.dest(buildConfig.rootJsFolder + 'angular2-busy/'));
});

gulp.task('copy-angular2-dynamic-component', function () {
    return gulp.src(buildConfig.sources.angular2DynamicComponent)
        .pipe(gulp.dest(buildConfig.rootJsFolder + 'angular2-dynamic-component/'));
});

gulp.task('copy-ts-metadata-helper', function () {
    return gulp.src(buildConfig.sources.tsMetadata)
        .pipe(gulp.dest(buildConfig.rootJsFolder + 'ts-metadata-helper/'));
});

gulp.task('copy-angular2-logger', function () {
    return gulp.src(buildConfig.sources.angular2Logger)
        .pipe(gulp.dest(buildConfig.rootJsFolder + 'angular2-logger/'));
});

gulp.task('copy-core-js', function () {
    return gulp.src(buildConfig.sources.coreJs)
        .pipe(gulp.dest(buildConfig.rootJsFolder + 'core-js/'));
});


gulp.task('copy-system-js', function () {
    return gulp.src('./systemjs.config.js')
        .pipe(gulp.dest(buildConfig.rootFolder, { "mode": "0777" }));
});

gulp.task('copy-index-html', function () {
    return gulp.src('./index.html')
        .pipe(gulp.dest(buildConfig.rootFolder, { "mode": "0777" }));
});

gulp.task('copy-toastr', function () {
    return gulp.src(buildConfig.sources.angularToastr)
        .pipe(gulp.dest(buildConfig.rootJsFolder + 'angular2-toaster/'));
});

gulp.task('copy-loadingBar', function () {
    return gulp.src(buildConfig.sources.angularLoadingBar)
        .pipe(gulp.dest(buildConfig.rootJsFolder + 'ng2-loading-bar/'));
});

gulp.task('copy-allOther', function () {
    return gulp.src(buildConfig.sources.jsFilesInclSourcePaths)
        .pipe(gulp.dest(buildConfig.rootJsFolder));
});

gulp.task('copy-vendor-css-to-wwwroot', function () {
    return gulp.src(buildConfig.sources.cssFiles)
        .pipe(gulp.dest(buildConfig.rootCssFolder));
});

gulp.task('start-watch', function () {
    gulp.watch([
        './angularApp/**/*.js',
        './angularApp/**/*.html',
        './angularApp/**/*.css',
    ], ['copy-app']);
});
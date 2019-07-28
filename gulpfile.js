'use strict';

//includes
var gulp = require('gulp'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify'),
    cleancss = require('gulp-clean-css'),
    less = require('gulp-less'),
    rename = require('gulp-rename'),
    replace = require('gulp-replace'),
    fs = require('fs'),
    modify = require('gulp-modify-file');

//determine environment
var prod = false;
var debug = true;

//paths
var paths = {
    scripts: 'App/Scripts/',
    css: 'App/CSS/',
    app: 'App/',
    webroot: 'App/wwwroot/',
};
paths.gmail = paths.scripts + 'gmail/';

//get gmail version
var gmail_version = require('./' + paths.gmail + 'version.json');

//working paths
paths.working = {
    js: {
        platform: [
            paths.webroot + 'js/selector.js',
            paths.scripts + 'utility/velocity.min.js',
            paths.scripts + "platform/_super.js",
            paths.scripts + "platform/ajax.js",
            //paths.scripts + "platform/drag.js",
            paths.scripts + "platform/loader.js",
            paths.scripts + "platform/message.js",
            //paths.scripts + "platform/polyfill.js",
            paths.scripts + "platform/popup.js",
            paths.scripts + "platform/scaffold.js",
            //paths.scripts + "platform/scrollbar.js",
            paths.scripts + "platform/svg.js",
            paths.scripts + "platform/util.js",
            //paths.scripts + "platform/util.color.js",
            //paths.scripts + "platform/util.text.js",
            paths.scripts + "platform/validate.js",
            paths.scripts + "platform/window.js"
        ],
        gmail: [
            paths.webroot + 'js/selector.js',
            // Services ////////////////////////////////
            paths.gmail + 'Services/authenticate.js',
            paths.gmail + 'Services/plans.js', 
            paths.gmail + 'Services/subscription.js', 
            paths.gmail + 'Services/addressbook.js', 
            paths.gmail + 'Services/support.js', 
            // UI //////////////////////////////////////
            paths.gmail + 'UI/nav-menu.js', // nav menu
            paths.gmail + 'UI/modal.js', 
            paths.gmail + 'UI/message.js',
            paths.gmail + 'UI/app-toolbar.js',
            paths.gmail + 'UI/compose-view.js',
            // Pages ///////////////////////////////////
            paths.gmail + 'Pages/Subscription/addressbook.js', 
            paths.gmail + 'Pages/Subscription/campaigns.js', 
            paths.gmail + 'Pages/Subscription/reports.js', 
            paths.gmail + 'Pages/Subscription/settings.js', 
            paths.gmail + 'Pages/Subscription/team.js', 
            paths.gmail + 'Pages/Support/faqs.js', 
            paths.gmail + 'Pages/Support/getting-started.js', 
            paths.gmail + 'Pages/Support/subscriptions.js', 
            paths.gmail + 'Pages/Support/billing.js', 
            paths.gmail + 'Pages/Support/address-book.js', 
            paths.gmail + 'Pages/Support/import-data.js', 
            paths.gmail + 'Pages/Support/campaigns.js', 
            paths.gmail + 'Pages/Support/surveys.js', 
            paths.gmail + 'Pages/Support/reporting.js', 
            paths.gmail + 'Pages/Support/teams.js', 
            // Utility /////////////////////////////////
            paths.gmail + 'Utility/web.js',
            paths.gmail + 'Utility/email.js',
            paths.gmail + 'Utility/numbers.js',
        ],
        app: paths.app + '**/*.js',
        utility: [
            paths.scripts + 'utility/*.js',
            paths.scripts + 'utility/**/*.js'
        ]
    },

    less:{
        platform: paths.css + 'platform.less',
        app: [
            paths.app + '**/*.less'
        ],
        themes: paths.css + 'themes/*.less',
        tapestry: [
            paths.css + 'tapestry/tapestry.less',
            paths.css + 'tapestry/less/theme.less',
            paths.css + 'tapestry/less/util.less'
        ],
        utility: paths.css + 'utility/*.less'
    },

    css: {
        utility: paths.css + 'utility/**/*.css',
        app: paths.app + '**/*.css'
    },

    exclude: {
        app: [
            '!' + paths.app + 'wwwroot/**/*',
            '!' + paths.app + 'Content/**/*',
            '!' + paths.app + 'CSS/**/*',
            '!' + paths.app + 'CSS/*',
            '!' + paths.app + 'Scripts/**/*',
            '!' + paths.app + 'obj/**/*',
            '!' + paths.app + 'bin/**/*'
        ]
    }
};

//compiled paths
paths.compiled = {
    platform: paths.webroot + 'js/platform.js',
    gmail: paths.webroot + 'js/gmail.js',
    gmail_version: paths.webroot + 'js/gmail_version.js',
    js: paths.webroot + 'js/',
    css: paths.webroot + 'css/',
    app: paths.webroot + 'css/',
    themes: paths.webroot + 'css/themes/',
};

//tasks for compiling javascript //////////////////////////////////////////////////////////////
gulp.task('js:app', function () {
    var pathlist = paths.working.exclude.app.slice(0);
    pathlist.unshift(paths.working.js.app);
    var p = gulp.src(pathlist)
        .pipe(rename(function (path) {
            path.dirname = path.dirname.toLowerCase();
            path.basename = path.basename.toLowerCase();
            path.extname = path.extname.toLowerCase();
        }));

    if (prod == true) { p = p.pipe(uglify()); }
    return p.pipe(gulp.dest(paths.compiled.js, { overwrite: true }));
});

gulp.task('js:selector', function () {
    var p = gulp.src(paths.scripts + 'selector/selector.js', { base: '.' })
            .pipe(concat('selector.js'));
    if (prod == true) { 
        p = p.pipe(uglify());
    }
    return p.pipe(gulp.dest(paths.compiled.js, { overwrite: true }));
});

gulp.task('js:platform', function () {
    var p = gulp.src(paths.working.js.platform, { base: '.' })
        .pipe(concat(paths.compiled.platform));
    if (prod == true) { p = p.pipe(uglify()); }
    return p.pipe(gulp.dest('.', { overwrite: true }));
});

gulp.task('js:utility', function () {
    var p = gulp.src(paths.working.js.utility)
        .pipe(rename(function (path) {
            path.dirname = path.dirname.toLowerCase();
            path.basename = path.basename.toLowerCase();
            path.extname = path.extname.toLowerCase();
        }));

    if (prod == true) { p = p.pipe(uglify()); }
    return p.pipe(gulp.dest(paths.compiled.js + 'utility', { overwrite: true }));
});

gulp.task('version:gmail', function () {
    //increment version
    var watching = false; 
    var version_old = gmail_version.major + '.' + gmail_version.minor + '.' + gmail_version.dist;
    if (watching == false) {
        gmail_version.dist += 1;
    }
    var version_new = gmail_version.major + '.' + gmail_version.minor + '.' + gmail_version.dist;

    //copy config file(s)
    fs.writeFile(paths.scripts + 'gmail/version.txt', version_new, () => { });

    //update version.json
    if (watching == false) {
        fs.writeFile(paths.gmail + 'version.json', JSON.stringify(gmail_version), function (err) {
            if (err) { return console.log(err); }
            console.log('updated gmail version: ' + version_old + ' > ' + version_new);
        });
    }
    return gulp.src(paths.gmail + 'version.json');
});

gulp.task('js:gmail', gulp.series(
    () => {
        var p = gulp.src(paths.working.js.gmail, { base: '.' })
            .pipe(concat(paths.compiled.gmail));
        if (debug == false) {
            //remove debug info from files
            p = p.pipe(modify((content, path, file) => {
                const start = '//<debug>'
                const end = '//</debug>'
                var modified = content.toString();
                while (modified.length > 0) {
                    let next = modified.indexOf(start);
                    if (next >= 0) {
                        let last = modified.indexOf(end, next + 1);
                        if (last > 0) {
                            //found match, remove debug block from code
                            modified = modified.substr(0, next - 1) + modified.substr(last + end.length);
                        } else { break; }
                    } else { break; }
                }
                return modified;
            }));
        }
        if (prod == true) { p = p.pipe(uglify()); }
        return p.pipe(gulp.dest('.', { overwrite: true }));
    },
    'version:gmail')
);

gulp.task('js', gulp.series(
    'js:app',
    'js:selector',
    'js:platform',
    'js:utility',
    'js:gmail'
));

//tasks for compiling LESS & CSS /////////////////////////////////////////////////////////////////////
gulp.task('less:app', function () {
    var pathlist = paths.working.exclude.app.slice(0);
    for (var x = paths.working.less.app.length - 1; x >= 0; x--) {
        pathlist.unshift(paths.working.less.app[x]);
    }
    var p = gulp.src(pathlist)
        .pipe(less())
        .pipe(rename(function (path) {
            path.dirname = path.dirname.toLowerCase();
            path.basename = path.basename.toLowerCase();
            path.extname = path.extname.toLowerCase();
        }));
    if(prod == true){ p = p.pipe(cleancss({compatibility: 'ie8'})); }
    return p.pipe(gulp.dest(paths.compiled.app, { overwrite: true }));
});

gulp.task('less:platform', function () {
    var p = gulp.src(paths.working.less.platform)
        .pipe(less());
    if (prod == true) { p = p.pipe(cleancss({ compatibility: 'ie8' })); }
    return p.pipe(gulp.dest(paths.compiled.css, { overwrite: true }));
});

gulp.task('less:themes', function () {
    var p = gulp.src(paths.working.less.themes)
        .pipe(less());
    if (prod == true) { p = p.pipe(cleancss({ compatibility: 'ie8' })); }
    return p.pipe(gulp.dest(paths.compiled.css + 'themes', { overwrite: true }));
});

gulp.task('less:utility', function () {
    var p = gulp.src(paths.working.less.utility)
        .pipe(less());
    if (prod == true) { p = p.pipe(cleancss({ compatibility: 'ie8' })); }
    return p.pipe(gulp.dest(paths.compiled.css + 'themes', { overwrite: true }));
});

gulp.task('less:gmail', function () {
    var p = gulp.src(paths.css + 'gmail.less')
        .pipe(less());
    if (prod == true) { p = p.pipe(cleancss({ compatibility: 'ie8' })); }
    return p.pipe(gulp.dest(paths.css, { overwrite: true }));
});

gulp.task('css:app', function () {
    var pathlist = paths.working.exclude.app.slice(0);
    pathlist.unshift(paths.working.css.app);
    var p = gulp.src(pathlist)
        .pipe(rename(function (path) {
            path.dirname = path.dirname.toLowerCase();
            path.basename = path.basename.toLowerCase();
            path.extname = path.extname.toLowerCase();
        }));
    if (prod == true) { p = p.pipe(cleancss({ compatibility: 'ie8' })); }
    return p.pipe(gulp.dest(paths.compiled.app, { overwrite: true }));
});

gulp.task('css:utility', function () {
    var p = gulp.src(paths.working.css.utility)
        .pipe(rename(function (path) {
            path.dirname = path.dirname.toLowerCase();
            path.basename = path.basename.toLowerCase();
            path.extname = path.extname.toLowerCase();
        }));
    if (prod == true) { p = p.pipe(cleancss({ compatibility: 'ie8' })); }
    return p.pipe(gulp.dest(paths.compiled.css + 'utility', { overwrite: true }));
});

gulp.task('less', gulp.series(
    'less:platform',
    'less:app',
    'less:themes',
    'less:utility',
    'less:gmail'
));

gulp.task('css', gulp.series(
    'css:app',
    'css:utility'
));

//tasks for compiling vendor app dependencies /////////////////////////////////////////////////


//default task
gulp.task('default', gulp.series('js', 'less', 'css'));

//watch task
gulp.task('watch', function () {
    //watch platform JS
    gulp.watch(paths.working.js.platform, gulp.series('js:platform'));
    
    //watch app JS
    var pathjs = [paths.working.js.app, ...paths.working.exclude.app.map(a => a + '*.js')];
    gulp.watch(pathjs, gulp.series('js:app'));

    //watch gmail JS
    gulp.watch(paths.working.js.gmail, gulp.series('js:gmail'));
    
    //watch app LESS
    var pathless = [...paths.working.less.app, ...paths.working.exclude.app.map(a => a + '*.less')];
    gulp.watch(pathless, gulp.series('less:app'));

    //watch platform LESS
    gulp.watch([
        paths.working.less.platform,
        ...paths.working.less.tapestry
    ], gulp.series('less:platform'));

    //watch themes LESS
    gulp.watch([
        paths.working.less.themes
    ], gulp.series('less:themes', 'less:platform'));

    //watch utility LESS
    gulp.watch([
        paths.working.less.utility
    ], gulp.series('less:utility'));

    //watch gmail LESS
    gulp.watch([
        paths.css + 'gmail.less'
    ], gulp.series('less:gmail'));

    //watch app CSS
    var pathcss = [paths.working.css.app, ...paths.working.exclude.app.map(a => a + '*.css')];
    gulp.watch(pathcss, gulp.series('css:app'));
    
    //watch utility CSS
    gulp.watch([
        paths.working.css.utility
    ], gulp.series('css:utility'));
});
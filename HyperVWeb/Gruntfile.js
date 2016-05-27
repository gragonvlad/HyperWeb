/// <binding BeforeBuild='debug-Build' ProjectOpened='watch-Content' />
module.exports = function (grunt) {

    grunt.initConfig({
        concat: {
            options: {
                sourceMap: true
            },
            'dependencies-bundle': {
                src: [
                    'node_modules/angular-duration-format/dist/angular-duration-format.js',
                    
                ],
                dest: 'WebContent/Assets/dependencies-bundle.js',
                nonull: true
            },
        },
        'copy': {
            debug: {
                files: [
                  // includes files within path
                  { expand: true, src: ['WebContent/**'], dest: 'bin/Debug/', filter: 'isFile' }
                ]
            },
            release: {
                files: [
                  // includes files within path
                  { expand: true, src: ['WebContent/**'], dest: 'bin/Release/', filter: 'isFile' }
                ]
            },
        },
        'watch': {
            files: ['<%= copy.debug.files[0].src[0] %>'],
            tasks: ['newer:copy:debug']
        }
    });

    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-newer');

    grunt.registerTask('watch-Content', ['watch']);
    grunt.registerTask('debug-Build', ['newer:concat', 'newer:copy:debug']);
    grunt.registerTask('release-Build', ['newer:concat', 'newer:copy:release']);
};
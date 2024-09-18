'use strict';

/**
 * cadastre service
 */

const { createCoreService } = require('@strapi/strapi').factories;

module.exports = createCoreService('api::cadastre.cadastre');

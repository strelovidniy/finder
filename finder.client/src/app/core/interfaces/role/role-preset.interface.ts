interface IRolePreset {
    canDeleteUsers?: boolean;
    canEditUsers?: boolean;
    canCreateRoles?: boolean;
    canEditRoles?: boolean;
    canDeleteRoles?: boolean;
    canSeeAllUsers?: boolean;
    canSeeAdminRoles?: boolean;
    canSeeVendorRoles?: boolean;
    canSeeMonogramVendorRoles?: boolean;
    canSeeCustomArtworkVendorRoles?: boolean;
    canMaintainSystem?: boolean;
    canSeeQuestions?: boolean;
    canCreateQuestion?: boolean;
    canEditQuestion?: boolean;
    canDeleteQuestion?: boolean;
    canSeeWidgetConfigs?: boolean;
    canCreateWidgetConfig?: boolean;
    canEditWidgetConfig?: boolean;
    canDeleteWidgetConfig?: boolean;
    canSeeWidgetInstances?: boolean;
    canCreateWidgetInstance?: boolean;
    canEditWidgetInstance?: boolean;
    canDeleteWidgetInstance?: boolean;
}

export default IRolePreset;
